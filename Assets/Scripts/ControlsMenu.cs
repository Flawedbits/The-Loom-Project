using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    public SettingsMenu settingsMenuScript;
    public PauseMenu pauseMenuScript;
    public GameObject keyGUI;

    string update = "null";
    bool readKey = false;

    void Start()
    {
        AddGooglesBinding();
    }

    void AddGooglesBinding()
    {
        Transform existingBinding = transform.Find("GooglesKey");
        if (existingBinding != null)
        {
            return;
        }

        Transform grabBinding = transform.Find("GrabKey");
        Transform grabLabel = transform.Find("Grab");
        Button grabButton = grabBinding != null ? grabBinding.GetComponent<Button>() : null;
        TMP_Text labelText = grabLabel != null ? grabLabel.GetComponent<TMP_Text>() : null;
        if (grabButton == null || labelText == null)
        {
            Debug.LogError("ControlsMenu could not create the Googles binding because the Grab row is missing.", this);
            return;
        }

        GameObject bindingObject = Instantiate(grabButton.gameObject, transform, false);
        bindingObject.name = "GooglesKey";
        RectTransform bindingRect = bindingObject.GetComponent<RectTransform>();
        bindingRect.anchoredPosition = grabBinding.GetComponent<RectTransform>().anchoredPosition + Vector2.down * 40f;

        TMP_Text bindingText = bindingObject.GetComponentInChildren<TMP_Text>(true);
        if (bindingText == null)
        {
            Debug.LogError("ControlsMenu could not find the key label for the cloned Googles binding.", this);
            Destroy(bindingObject);
            return;
        }
        bindingText.gameObject.name = "GooglesKey";

        Button googlesButton = bindingObject.GetComponent<Button>();
        googlesButton.onClick = new Button.ButtonClickedEvent();
        googlesButton.onClick.AddListener(Googles);

        GameObject labelObject = Instantiate(grabLabel.gameObject, transform, false);
        labelObject.name = "Googles";
        labelObject.GetComponent<RectTransform>().anchoredPosition =
            grabLabel.GetComponent<RectTransform>().anchoredPosition + Vector2.down * 40f;
        labelObject.GetComponent<TMP_Text>().text = "Googles";
    }

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
                                    case "GooglesKey":
                                        CustomInputManager.IM.googles = keyCode;
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
        SaveData.saveControlsToJson(CustomInputManager.IM.forward, CustomInputManager.IM.backward, CustomInputManager.IM.left, CustomInputManager.IM.right, CustomInputManager.IM.jump, CustomInputManager.IM.sprint, CustomInputManager.IM.crouch, CustomInputManager.IM.grab, CustomInputManager.IM.googles);
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

    public void Googles()
    {
        update = "GooglesKey";
    }
}
