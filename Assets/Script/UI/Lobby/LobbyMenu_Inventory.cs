﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu_Inventory : MonoBehaviour, ILobbyMenu
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }
}
