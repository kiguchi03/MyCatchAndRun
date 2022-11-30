using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの動き、Rayを制御
/// </summary>
public class CameraControll : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    PlayerItemManager playerItemManager;

    [SerializeField]
    GameObject enemy;

    [SerializeField]
    UIManager ui;

    ISlipRay saveSlipRay;


    LayerMask layerMask = 2048;


    //X,Y軸のカメラ回転値
    float Xrotate;
    float Yrotate;

    float cameraAngleX;
    float cameraAngleY;

    bool isMove = true;

    public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }
    [Header("縦の動きの制限")]
    [SerializeField] float YrotateLimit = 45.0f;

    [Header("赤いバラ")]
    [SerializeField]
    Item itemRedRose;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        MoveCotroll();
    }

    // Update is called once per frame
    void Update()
    {
        RayControll();
    }

    /// <summary>
    /// カメラの動きを制御するメソッド
    /// </summary>
    private void MoveCotroll()
    {
        if (!isMove) return;

        Xrotate = Input.GetAxis("Mouse X") * MenuManager.instance.GetSensi;
        Yrotate = Input.GetAxis("Mouse Y") * MenuManager.instance.GetSensi;

        cameraAngleX += Mathf.Atan(Xrotate) * Mathf.Rad2Deg * 2.0f;
        cameraAngleY += Mathf.Atan(Yrotate) * Mathf.Rad2Deg * 2.0f;

        cameraAngleY = Mathf.Clamp(cameraAngleY, -YrotateLimit, YrotateLimit);

        gameObject.transform.localEulerAngles = new Vector3(cameraAngleY, cameraAngleX, 0);
    }

    /// <summary>
    /// オブジェクトを検知するメソッド
    /// </summary>
    private void RayControll()
    {
        if (Time.timeScale == 0.0f) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, this.gameObject.transform.forward, out hit, 1))
        {
            IRecieveKey recieveKey = hit.collider.GetComponent<IRecieveKey>();
            if (recieveKey != null)
            {
                ui.Show<EKeyText>();
                if (Input.GetKeyUp(KeyCode.E))
                {
                    recieveKey.RecieveKey();
                }
            }

            ItemManager itemManager = hit.collider.GetComponent<ItemManager>();
            if (itemManager != null)
            {
                ui.Show<EKeyText>();
                if (Input.GetKeyUp(KeyCode.E))
                {
                    playerItemManager.AddItemToPlayer(itemManager.GetItemID());
                }
            }
        }
        else
        {
            ui.Hide<EKeyText>();
        }

        //Enemy検知専用
        if (Physics.Raycast(transform.position, this.gameObject.transform.forward, out hit, 7, layerMask))
        {
            IRecieveRay recieveRay = hit.collider.GetComponent<IRecieveRay>();
            ISlipRay slipRay = hit.collider.GetComponent<ISlipRay>();

            if (recieveRay != null)
            {
                recieveRay.RecieveRay();
                saveSlipRay = slipRay;
            }

            if (recieveRay == null && saveSlipRay != null)
            {
                saveSlipRay.SlipRay();
                saveSlipRay = null;
            }
        }
    }
}
