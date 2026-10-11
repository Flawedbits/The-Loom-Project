using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    public static CustomInputManager IM;

    public KeyCode forward {get; set;}
    public KeyCode backward { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode sprint { get; set; }
    public KeyCode crouch { get; set; }
    public KeyCode grab { get; set; }

    private void Awake()
    {
        if(IM == null)
        {
            DontDestroyOnLoad(gameObject);
            IM = this;
        }
        else if(IM != this)
        {
            Destroy(gameObject);
        }
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey","W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprintKey", "LeftShift"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("crouchKey", "LeftControl"));
        grab = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("grabKey", "E"));

        string filePath = Application.persistentDataPath + "/ControlsData.json";
        if (System.IO.File.Exists(filePath))
        {
            CustomInputManager.IM.forward = SaveData.loadControlsFromJson()[0];
            CustomInputManager.IM.backward = SaveData.loadControlsFromJson()[1];
            CustomInputManager.IM.left = SaveData.loadControlsFromJson()[2];
            CustomInputManager.IM.right = SaveData.loadControlsFromJson()[3];
            CustomInputManager.IM.jump = SaveData.loadControlsFromJson()[4];
            CustomInputManager.IM.sprint = SaveData.loadControlsFromJson()[5];
            CustomInputManager.IM.crouch = SaveData.loadControlsFromJson()[6];
            CustomInputManager.IM.grab = SaveData.loadControlsFromJson()[7];
        }
    }
}
