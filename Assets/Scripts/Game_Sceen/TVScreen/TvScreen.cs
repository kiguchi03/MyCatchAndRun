using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テレビのイベント、マテリアル、オーディオを制御
/// </summary>
public class TvScreen :BaseObj,IRecieveKey
{
    
    [SerializeField]
    EnemyManager enemyManager;
    
    [SerializeField]
    VaseManager vaseManager;

    [SerializeField]
    RoseSpawn_GM roseSpawn_GM;

    [SerializeField]
    UIManager ui;

    Renderer matRender;

    [SerializeField]
    AudioClip noise;

    [Header("ブラックマテリアル")]
    [SerializeField]
    Material BlackScreen;

    [Header("ノイズマテリアル")]
    [SerializeField]
    Material NoiseScreen;

    [Header("最初のメッセージ")]
    [SerializeField]
    string firstMessage;

    //表示するメッセージ
    string message;

    //テレビスクリーンをノイズにするか
    bool isNoisy;

    //基準となる音量
    float baseVolume;

    //最初のイベントが終わってるか
    static bool isFirstEvent;

    public override void Init()
    {
        matRender = GetComponent<Renderer>();
        if (!isFirstEvent) TvFirstEvent();
    }

    public override void ManagedUpdate()
    {
        ScreenCon();

        if(enemyManager.EnemyFadeValue == enemyManager.enemyPowerUpValue)
        {
            vaseManager.TrueRoseFlag();

            message = vaseManager.message;

            isNoisy = true;

            enemyManager.ResetFadeValue();
        }
    }

    /// <summary>
    /// ゲーム開始直後テレビをノイズにし、敵のスポーンを止めるメソッド
    /// </summary>
    private void TvFirstEvent()
    {
        isNoisy = true;

        message = firstMessage;
        
        enemyManager.noSpawn = true;
        
        isFirstEvent = true;
    }

    /// <summary>
    /// メッセージを表示し、時間を停止させるメソッド
    /// </summary>
    public void RecieveKey()
    {
        ui.Show<EKeyText>();

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isNoisy)
            {
                ui.message = message;

                ui.Show<Message>();

                TimeManager.instance.StopTime();

                isNoisy = false;
            }
        }
    }

    /// <summary>
    /// テレビスクリーンのマテリアルをコントロールするメソッド
    /// </summary>
    private void ScreenCon()
    {
        
        //テレビスクリーンをノイズマテリアルにする
        if (isNoisy && enemyManager.noSpawn && matRender.sharedMaterial != NoiseScreen)
        {
            matRender.material = NoiseScreen;

            SoundManager.instance.PlayBGM(noise);
        }
        
        //メッセージ表示中にEキーを押すとメッセージを閉じ、テレビスクリーンをブラックマテリアルにし、敵のスポーンを開始させる
        if (!isNoisy && Input.GetKeyUp(KeyCode.E) && matRender.sharedMaterial != BlackScreen)
        {
            TimeManager.instance.StartTime();

            SoundManager.instance.StopBGM();

            matRender.material = BlackScreen;
            
            enemyManager.noSpawn = false;

            ui.Hide<Message>();
        }
    }
}
