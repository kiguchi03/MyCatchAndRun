using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObj : MonoBehaviour,IManagedUpdate,I_Init
{
    public abstract void Init();

    public abstract void ManagedUpdate();
}
