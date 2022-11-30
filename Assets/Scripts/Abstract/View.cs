using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour,IManagedUpdate,I_Init
{
    public virtual void Init()
    {

    }

    public virtual void ManagedUpdate()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
