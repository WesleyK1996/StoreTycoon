using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GameObject currentMenu;

    private void Start()
    {
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

    public void ToggleBuild()
    {
        GameObject go = transform.Find("Build").gameObject;
        go.SetActive(!go.activeSelf);
        if((Cursor.lockState == CursorLockMode.Locked && go.activeSelf == true) || (Cursor.lockState == CursorLockMode.None && go.activeSelf == false))
        ToggleCursor();
    }

}
