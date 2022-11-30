using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : View
{
    [SerializeField]
    UIManager ui;

    public Text text { get; internal set; }

    public override void Init()
    {
        text = GetComponentInChildren<Text>();

        Hide();
    }

    public override void Show()
    {
        text.text = ui.message;

        base.Show();
    }
}
