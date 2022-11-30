﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseSpawnPosition : MonoBehaviour
{
    bool isEnter;

    bool isExit;

    bool isStay;

    bool inPlayer;

    string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isExit = true;
        }
    }

    public bool GetInPlayer()
    {
        if (isExit)
        {
            inPlayer = false;
        }
        else if (isEnter || isStay)
        {
            inPlayer = true;
        }

        isEnter = false;
        isStay = false;
        isExit = false;

        return inPlayer;
    }
}
