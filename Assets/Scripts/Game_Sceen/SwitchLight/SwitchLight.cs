using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : MonoBehaviour,IRecieveKey
{
    [SerializeField]
    private Light _light;

    [Header("押した音")]
    [SerializeField]
    AudioClip push;

    public void RecieveKey()
    {
        _light.enabled = !_light.enabled!;

        SoundManager.instance.PlaySE(push);
    }  
}
