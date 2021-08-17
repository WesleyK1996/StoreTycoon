using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    ScrollRect tabs;
    GameObject currentMenu;

    private void Start()
    {
        tabs = transform.Find("Build").Find("Scroll View Tabs").GetComponent<ScrollRect>();
        currentMenu = transform.Find("Main").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && currentMenu.name == "Main") ToggleCursor();
        if (Input.GetKeyDown(KeyCode.B)) ToggleBuild();
    }
    private void ToggleCursor()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ToggleBuild()
    {
        ToggleCursor();
        GameObject go = transform.Find("Build").gameObject;
        go.SetActive(!go.activeSelf);
    }

}
