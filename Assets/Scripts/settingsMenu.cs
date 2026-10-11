using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public PauseMenu pauseMenuScript;
    public GameObject volumeMenuUI;
    public GameObject controlsMenuUI;

    public static bool volumeOpen = false;
    public static bool controlsOpen = false;

    void Update()
    {
        if (PauseMenu.settingsOpen)
        {
            pauseMenuScript.enabled = false;

            if (Input.GetButtonDown("Pause"))
            {
                Back();
            }
        }
    }

    public void Back()
    {
        pauseMenuScript.enabled = true;
        pauseMenuScript.pauseMenuUi.SetActive(true);
        pauseMenuScript.settingsMenuUI.SetActive(false);
        PauseMenu.settingsOpen = false;
    }

    public void Volume()
    {
        pauseMenuScript.settingsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(true);
        volumeOpen = true;
    }

    public void Controls()
    {
        pauseMenuScript.settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        controlsOpen = true;
    }
}
