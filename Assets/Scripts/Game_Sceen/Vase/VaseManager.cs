using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseManager : MonoBehaviour
{
    [SerializeField]
    List<VaseController> vases = new List<VaseController>();

    public List<VaseController> GetVases
    {
        get { return vases; }
    }

    public string message { get; private set; }

    [SerializeField]
    List<Item> items = new List<Item>();

    public List<Item> GetItems
    {
        get { return items; }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var vase in vases)
        {
            vase.Init();
        }
    }

    private void FixedUpdate()
    {
        foreach(var vase in vases)
        {
            vase.ManagedUpdate();
        }
    }

    public void TrueRoseFlag()
    {
        foreach(var item in items)
        {
            if (!item.GetFlagData.GetSetIsBool)
            {
                item.GetFlagData.GetSetIsBool = true;

                message = item.GetRoseMessage;

                break;
            }
        }
    }
}
