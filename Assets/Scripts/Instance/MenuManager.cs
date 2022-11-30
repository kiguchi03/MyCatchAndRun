using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    //メニューパネル
    GameObject menuPanel;

    //音量スライダー
    Slider volumeSlider;
    //カメラ感度スライダー
    Slider sensiSlider;

    //音量値
    float volumeValue = 0.5f;
    //感度値
    float sensiValue = 0.05f;


    public float GetVolume
    {
        get { return volumeValue; }
    }

    public float GetSensi
    {
        get { return sensiValue; }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //シーン開始直後にメニュパネルを非表示にし、スライダーの値に音量・感度値を代入
        if (menuPanel == null)
        {
            menuPanel = GameObject.FindGameObjectWithTag("MenuPanel");

            menuPanel.SetActive(false);

            volumeSlider = (menuPanel.transform.GetChild(0).GetComponent<Slider>());
            sensiSlider = (menuPanel.transform.GetChild(1).GetComponent<Slider>());

            volumeSlider.value = volumeValue;
            sensiSlider.value = sensiValue;
        }
        
        //メニューパネルのスライダーの値を保存
        volumeValue = volumeSlider.value;
        sensiValue = sensiSlider.value;
    }
}
