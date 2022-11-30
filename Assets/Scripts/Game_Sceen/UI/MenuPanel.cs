using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : View
{
    public override void ManagedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public override void Show()
    {
        if (Time.timeScale == 0.0f) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TimeManager.instance.StopTime();

        base.Show();
    }

    public override void Hide()
    {
        if (Time.timeScale == 1.0f) return;

        TimeManager.instance.StartTime();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        base.Hide();
    }
}
