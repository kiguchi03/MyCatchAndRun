using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    internal List<Enemy> enemyList = new List<Enemy>();

    [SerializeField]
    internal VaseManager vaseManager;

    [SerializeField]
    internal PlayerItemManager playerItemManager;

    [SerializeField]
    internal UIManager ui;

    [Header("ライトオブジェクト")]
    [SerializeField]
    internal List<Light> lights = new List<Light>();

    
    [SerializeField]
    List<FlagData> roseFlags = new List<FlagData>();

    [SerializeField]
    List<Item> items = new List<Item>();
    
    public int enemyPowerUpValue;

    public int EnemyFadeValue { get; private set; }

    public bool isSpanwed { get; set; }

    public bool noSpawn { get;set; }

    private void FixedUpdate()
    {
        foreach(var enemy in enemyList)
        {
            enemy.ManagedUpdate();
        }

        if(EnemyFadeValue == enemyPowerUpValue)
        {
            foreach(var item in items)
            {
                if (item.GetFlagData.GetSetIsBool)
                {
                    item.GetFlagData.GetSetIsBool = true;
                }
            }
        }
    }

    internal void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
        enemy.Init();
    }

    internal void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        EnemyFadeValue++;
        enemy.Destroy();

        if(EnemyFadeValue == enemyPowerUpValue)
        {
            noSpawn = true;
        }
    }

    public void ResetFadeValue()
    {
        EnemyFadeValue = 0;
    }
}
