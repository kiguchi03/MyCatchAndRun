using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    List<Player> players = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var player in players)
        {
            player.Init();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var player in players)
        {
            player.ManagedUpdate();
        }
    }
}
