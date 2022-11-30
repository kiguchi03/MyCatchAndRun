using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GirlState;

/// <summary>
/// 敵の能力、消滅、オーディオ、赤いバラの受け取りの制御
/// </summary>
public class GirlControll : Enemy,IRecieveKey,IRecieveRay,ISlipRay
{
    GirlControll enemyControll;
    
    public List<VaseController> vaseControllers = new List<VaseController>();
    
    VaseManager vaseManager;

    public PlayerItemManager playerItemManager;

    internal UIManager ui;

    internal NavMeshAgent navMeshAgent;

    public StateMachine<GirlControll> stateMachine { get; private set; }

    Animator anim;

    //敵が消すライト
    public List<Light> lights = new List<Light>();

    [Tooltip("スポーンする時のボイス")]
    [SerializeField]
    AudioClip enemySpawnVoice;

    [Tooltip("消滅する時のボイス")]
    [SerializeField]
    AudioClip enemyFadeOutVoice;

    [Tooltip("クリア条件を満たした時のボイス")]
    [SerializeField]
    AudioClip enemyClearVoice;

    [Tooltip("消滅する際のパーティクル")]
    [SerializeField]
    GameObject fadeOutVFX;

    [SerializeField]
    public bool isRendered { get; private set; }

    //カメラのレイを受付けないか
    public bool noRecieveRender { get; private set; }

    public bool isAct { get; set; }

    //時間経過
    float seconds;

    [Header("敵が消滅する制限時間")]
    [SerializeField]
    float limitSeconds = 15.0f;

    [Header("強化時の一度で消すライトの数")]
    [SerializeField]
    int lightControllValue = 2;

    [Header("強化時のスピードアップの倍率")]
    [SerializeField]
    [Range(1.0f, 2.0f)] float Speed_Increase;

    [Header("強化時のスピードダウンの倍率")]
    [SerializeField]
    [Range(0.1f, 0.9f)] float Speed_DecreaseRate;

    [Header("赤いバラ")]
    [SerializeField]
    Item itemRedRose;

    [Tooltip("敵の手の赤バラ")]
    [SerializeField]
    GameObject objRedRose;

    [SerializeField]
    FlagData GameClear;

    public override void Init()
    {
        stateMachine = new StateMachine<GirlControll>(this);

        stateMachine.AddTransition<Stop, Chase>((int)GirlStates.Chase);
        stateMachine.AddTransition<Chase, Stop>((int)GirlStates.Stop);

        stateMachine.Start<Stop>();


        lights = enemyManager.lights;

        vaseManager = enemyManager.vaseManager;

        playerItemManager = enemyManager.playerItemManager;

        ui = enemyManager.ui;

        target = GameObject.FindGameObjectWithTag("Player");

        enemyControll = GetComponent<GirlControll>();

        anim = GetComponent<Animator>();

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        objRedRose.SetActive(false);


        SoundManager.instance.PlaySE(enemySpawnVoice);

        EnemyAbility();
    }

    public override void ManagedUpdate()
    {
        stateMachine.Update();

        seconds += Time.fixedDeltaTime;

        //制限時間に達すると消滅
        if (seconds >= limitSeconds && !isAct)
        {
            StartCoroutine("enemyFadeOut");
            isAct = true;
        }
    }

    public override void Destroy()
    {
        enemyManager.isSpanwed = false;

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 強化時の敵の能力を制御するメソッド
    /// </summary>
    private void EnemyAbility()
    {
        foreach(var vase in vaseManager.GetVases)
        {
            if(!vase.isSetRose && vase.GetItemRose.GetFlagData.GetSetIsBool)
            {
                switch (vase.GetItemRose.GetRoseColor)
                {
                    case Item.Color.Black:

                        LightsControll();

                        break;

                    case Item.Color.Blue:

                        noRecieveRender = true;

                        navMeshAgent.speed = navMeshAgent.speed * Speed_DecreaseRate;

                        break;

                    case Item.Color.Purple:

                        navMeshAgent.speed = navMeshAgent.speed * Speed_Increase;

                        break;
                }
            }
        }
    }

    /// <summary>
    /// アクティブなライトを探し、任意の数のライトをランダムに非アクティブにするメソッド　強化時に呼び出される
    /// </summary>
    private void LightsControll()
    {
        //アクティブなライト格納するリスト shiningLightlist
        List<Light> shiningLightlist = new List<Light>();

        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i].enabled)
            {
                shiningLightlist.Add(lights[i]);
            }
        }

        //shiningLightListのランダムなライトを非アクティブ化
        for (int i = 0; i < lightControllValue; i++)
        {
            if (shiningLightlist.Count > 0)
            {
                int randomValue = Random.Range(0, shiningLightlist.Count);
                shiningLightlist[randomValue].enabled = false;
                shiningLightlist.RemoveAt(randomValue);
            }
        }
    }

    /// <summary>
    /// 敵が消滅する際のアクションを制御するメソッド
    /// </summary>
    /// <returns></returns>
    public IEnumerator enemyFadeOut()
    {
        SoundManager.instance.PlaySE(enemyFadeOutVoice);

        yield return new WaitForSeconds(3);

        EnemyEffect(this.gameObject.transform.position, fadeOutVFX);

        enemyManager.RemoveEnemy(this);
    }

    /// <summary>
    /// 任意の場所に任意のエフェクトを発生させるメソッド
    /// </summary>
    /// <param name="pos">場所</param>
    /// <param name="effect">エフェクト</param>
    private void EnemyEffect(Vector3 pos, GameObject effect)
    {
        Instantiate(effect, pos, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
    }

    public void RecieveKey()
    {
        foreach (var item in playerItemManager.GetHaveItemList)
        {
            if (item == itemRedRose)
            {
                ui.Show<EKeyText>();

                if (Input.GetKeyUp(KeyCode.E))
                {
                    this.objRedRose.SetActive(true);

                    this.isAct = true;

                    stateMachine.OfferTransition((int)GirlStates.Stop);

                    this.StopCorou();

                    anim.SetBool("GetRose", true);
                }
            }
        }
    }

    public void RecieveRay()
    {
        isRendered = true;
    }

    public void SlipRay()
    {
        isRendered = false;
    }

    /// <summary>
    /// クリア時用の消滅メソッド
    /// GetRose Animationで呼び出し
    /// </summary>
    public void Enemy_Clear()
    {
        EnemyEffect(this.gameObject.transform.position, fadeOutVFX);

        GameClear.GetSetIsBool = true;

        enemyManager.RemoveEnemy(this);
    }

    /// <summary>
    /// クリア時用のボイスメソッド
    /// GetRose Animationで呼び出し
    /// </summary>
    public void ClearVoice()
    {
        SoundManager.instance.PlaySE(enemyClearVoice);
    }

    /// <summary>
    /// コルーチンを停止するメソッド
    /// </summary>
    private void StopCorou()
    {
        StopCoroutine("enemyFadeOut");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAct)
        {
            Attack();
        }
    }
}