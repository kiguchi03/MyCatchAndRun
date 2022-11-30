using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//タイトルでのUI、シーン遷移を制御
public class TitleManager : MyScene
{
    [SerializeField]
    EventSystem eventSystem;

    [Header("タイトルBGM")]
    [SerializeField]
    AudioClip titleBGM;

    [Header("最初に選択されるボタン")]
    [SerializeField]
    Button firstSelectButton;

    [Header("選択されたくないUIオブジェクト")]
    [SerializeField]
    List<GameObject> notSelected = new List<GameObject>();

    //直後に選択されていたUIオブジェクト
    GameObject previous;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        firstSelectButton.Select();

        FadeManager.instance.FadeIn(fadeInTime);

        SoundManager.instance.PlayBGM(titleBGM);
    }

    private void Update()
    {
        //MenuPanelのスライダーをマウスで選択してもボタンからフォーカスが外れないようにする
        if ((previous != eventSystem.currentSelectedGameObject || previous == null) &&
            !notSelected.Contains(eventSystem.currentSelectedGameObject) && eventSystem.currentSelectedGameObject != null)
        {
            previous = eventSystem.currentSelectedGameObject;
        }

        if (eventSystem.currentSelectedGameObject == null || notSelected.Contains(eventSystem.currentSelectedGameObject))
        {
            EventSystem.current.SetSelectedGameObject(previous);
        }
    }

    /// <summary>
    /// GameSceneに遷移するメソッド
    /// </summary>
    public void StartButton()
    {
        if (FadeManager.instance.notInput) return;

        FadeManager.instance.FadeOut(fadeOutTime);

        StartCoroutine(SceneLoad(fadeOutTime, 1));
    }
}
