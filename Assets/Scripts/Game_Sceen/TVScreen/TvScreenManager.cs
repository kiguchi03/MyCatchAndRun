using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvScreenManager : MonoBehaviour
{
    [SerializeField]
    List<BaseObj> tvs = new List<BaseObj>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var tv in tvs)
        {
            tv.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var tv in tvs)
        {
            tv.ManagedUpdate();
        }
    }
}
