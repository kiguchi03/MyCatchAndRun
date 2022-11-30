using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

/// <summary>
/// フラグ、タイムライン、シーン遷移、タイムスケールを制御
/// </summary>
public class GameScene : MyScene
{
    [SerializeField]
    EnemyManager enemyManager;

    RoseSpawn_GM roseSpawn_;

    [SerializeField]
    TvScreenManager tvScreenManager;


    [Header("敵がプレイヤーを襲うタイムラインオブジェクト")]
    [SerializeField]
    PlayableDirector EnemyTimeLine;

    [Header("ブラックアウトのパネル")]
    [SerializeField]
    GameObject BlackPanel;

    [Header("バラ")]
    [SerializeField]
    List<Item> items = new List<Item>();

    [SerializeField]
    FlagData gameOver;

    [SerializeField]
    FlagData gameClear;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        BlackPanel.SetActive(false);

        roseSpawn_ = GetComponent<RoseSpawn_GM>();


        FadeManager.instance.FadeIn(1.0f);

        gameClear.GetSetIsBool = false;
        gameOver.GetSetIsBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver.GetSetIsBool)
        {
            StartCoroutine(BlackOut((float)EnemyTimeLine.duration));
        }

        GameClear();
    }

    /// <summary>
    /// 任意の秒数の遅延後にブラックパネルを表示するメソッド
    /// 敵に襲われた時に呼び出し
    /// </summary>
    /// <param name="sec">遅延時間</param>
    /// <returns></returns>
    private IEnumerator BlackOut(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);

        BlackPanel.SetActive(true);

        StartCoroutine(SceneLoad(2.0f, 1));
    }

    /// <summary>
    /// ゲームクリアした際に呼び出されるメソッド
    /// </summary>
    private void GameClear()
    {
        if (gameClear.GetSetIsBool)
        {
            enemyManager.noSpawn = true;

            FadeManager.instance.FadeOut(0.3f);

            StartCoroutine(SceneLoad(5.0f, 0));
        }
    }
}