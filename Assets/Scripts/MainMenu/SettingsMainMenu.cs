using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMainMenu : MonoBehaviour
{
    public MainMenu mainMenuScript;
    public GameObject volumeMenuUI;
    public GameObject controlsMenuUI;

    public static bool volumeOpen = false;
    public static bool controlsOpen = false;

    void Update()
    {
        if (MainMenu.settingsOpen)
        {
            if (Input.GetButtonDown("Pause"))
            {
                Back();
            }
        }
    }

    public void Back()
    {
        mainMenuScript.mainMenuUi.SetActive(true);
        mainMenuScript.settingsMenuUI.SetActive(false);
        MainMenu.settingsOpen = false;
    }

    public void Volume()
    {
        mainMenuScript.settingsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(true);
        volumeOpen = true;
    }

    public void Controls()
    {
        mainMenuScript.settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        controlsOpen = true;
    }
}
