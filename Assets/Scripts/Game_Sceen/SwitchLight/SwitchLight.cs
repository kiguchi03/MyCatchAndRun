using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : MonoBehaviour,IRecieveKey
{
    [SerializeField]
    private Light _light;

    [Header("ζΌγγι³")]
    [SerializeField]
    AudioClip push;

    public void RecieveKey()
    {
        _light.enabled = !_light.enabled!;

        SoundManager.instance.PlaySE(push);
    }  
}
