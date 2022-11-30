using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour,I_Init,IManagedUpdate
{
    public abstract void Init();

    public abstract void ManagedUpdate();
}
