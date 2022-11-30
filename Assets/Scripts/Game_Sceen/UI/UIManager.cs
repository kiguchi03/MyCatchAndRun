using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<View> views;

    public string message { get; set; }

    public void Show<View>()
    {
        if (FadeManager.instance.notInput) return;

        foreach (var view in views)
        {
            if(view is View)
            {
                view.Show();
            }
        }
    }
    public void Hide<View>()
    {
        if (FadeManager.instance.notInput) return;

        foreach (var view in views)
        {
            if (view is View)
            {
                view.Hide();
            }
        }
    }

    void Start()
    {
        foreach(var view in views)
        {
            view.Init();
        }  
    }

    void Update()
    {
        if (FadeManager.instance.notInput) return;

        foreach(var view in views)
        {
            view.ManagedUpdate();
        }    
    }
}
