using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 敵スポーンの開始、停止を制御
/// </summary>
public class SpawnerControll : MonoBehaviour
{
    SpawnerControll spawnerControll;

    [SerializeField]
    EnemyManager enemyManager;

    [Header("スポーンさせる敵")]
    [SerializeField]
    List<GameObject> targetList = new List<GameObject>();

    [Header("敵のスポーン位置を示す空のオブジェクト")]
    [SerializeField]
    List<GameObject> spawnPosition = new List<GameObject>();

    [SerializeField]
    PlayableDirector TimeLine;

    [SerializeField]
    GameObject Enemy_StartPos_TimeLine;


    [Header("敵スポーンの間隔")]
    [SerializeField]
    float SpawnDelayTime = 20.0f;

    float seconds = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnerControll = GetComponent<SpawnerControll>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyManager.noSpawn) return;

        //secondsが0.0になれば敵をスポーンさせる
        if (!enemyManager.isSpanwed)
        {
            seconds -= Time.fixedDeltaTime;

            if (seconds > 0.0f) return;

            GenerateEnemy();

            seconds = SpawnDelayTime;
        }
    }

    /// <summary>
    /// 敵をランダムなspawnPositionの一箇所からスポーンさせる
    /// </summary>
    void GenerateEnemy()
    {
        //敵がスポーン可能なspawnPositionを格納するリスト　spawnList
        List<GameObject> spawnPosList = new List<GameObject>();

        //プレイヤーが範囲外にいるSpawnPositionをspawnListに追加
        for (int i = 0; i < spawnPosition.Count; i++)
        {
            if (!spawnPosition[i].GetComponent<SpawnPosition>().GetInPlayer())
            {
                spawnPosList.Add(spawnPosition[i]);
            }
        }

        int randomSpawnPos = Random.Range(0, spawnPosList.Count);

        int randomSpawnEnemy = Random.Range(0, targetList.Count);


        //敵をスポーンし、スポーンした敵に必要なコンポーネントをアタッチする

        GameObject enemyPrefab = Instantiate(targetList[randomSpawnEnemy],spawnPosList[randomSpawnPos].transform.position,
            Quaternion.identity);

        Enemy enemy = enemyPrefab.GetComponent<Enemy>();

        enemy.enemyManager = enemyManager;

        enemyManager.AddEnemy(enemy);

        enemy.TimeLine = TimeLine;

        enemyManager.isSpanwed = true;
    }
}
