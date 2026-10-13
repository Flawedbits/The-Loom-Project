using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsMenu : MonoBehaviour
{
    public SettingsMenu settingsMenuScript;
    public PauseMenu pauseMenuScript;
    public GameObject keyGUI;

    string update = "null";
    bool readKey = false;

    void Update()
    {
        if (SettingsMenu.controlsOpen)
        {
            settingsMenuScript.enabled = false;

            if(update != "null") {
                readKey = true;
                keyGUI.SetActive(true);
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            if (keyCode != KeyCode.Escape)
                            {
                                switch (update)
                                {
                                    case "ForwardKey":
                                        CustomInputManager.IM.forward = keyCode;
                                        break;
                                    case "BackwardKey":
                                        CustomInputManager.IM.backward = keyCode;
                                        break;
                                    case "LeftKey":
                                        CustomInputManager.IM.left = keyCode;
                                        break;
                                    case "RightKey":
                                        CustomInputManager.IM.right = keyCode;
                                        break;
                                    case "JumpKey":
                                        CustomInputManager.IM.jump = keyCode;
                                        break;
                                    case "SprintKey":
                                        CustomInputManager.IM.sprint = keyCode;
                                        break;
                                    case "CrouchKey":
                                        CustomInputManager.IM.crouch = keyCode;
                                        break;
                                    case "GrabKey":
                                        CustomInputManager.IM.grab = keyCode;
                                        break;
                                }
                            }
                            update = "null";
                        }
                    }
                }
            }
            if (!readKey)
            {
                if (Input.GetButtonDown("Pause"))
                {
                    Back();
                }
                keyGUI.SetActive(false);
            }
            readKey = false;
        }
    }

    public void Back()
    {
        SaveData.saveControlsToJson(CustomInputManager.IM.forward, CustomInputManager.IM.backward, CustomInputManager.IM.left, CustomInputManager.IM.right, CustomInputManager.IM.jump, CustomInputManager.IM.sprint, CustomInputManager.IM.crouch, CustomInputManager.IM.grab);
        settingsMenuScript.enabled = true;
        pauseMenuScript.settingsMenuUI.SetActive(true);
        settingsMenuScript.controlsMenuUI.SetActive(false);
        SettingsMenu.controlsOpen = false;
    }

    public void Forward()
    {
        update = "ForwardKey";
    }

    public void Backward()
    {
        update = "BackwardKey";
    }

    public void Left()
    {
        update = "LeftKey";
    }

    public void Right()
    {
        update = "RightKey";
    }

    public void Jump()
    {
        update = "JumpKey";
    }

    public void Sprint()
    {
        update = "SprintKey";
    }

    public void Crouch()
    {
        update = "CrouchKey";
    }

    public void Grab()
    {
        update = "GrabKey";
    }
}
