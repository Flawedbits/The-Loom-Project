using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetKeyName : MonoBehaviour
{
    TMP_Text text;

    void Start()
    {
        // Get the TMP_Text component attached to the same GameObject
        text = GetComponent<TMP_Text>();
        switch (text.name)
        {
            case "ForwardKey":
                text.text = CustomInputManager.IM.forward.ToString();
                break;
            case "BackwardKey":
                text.text = CustomInputManager.IM.backward.ToString();
                break;
            case "LeftKey":
                text.text = CustomInputManager.IM.left.ToString();
                break;
            case "RightKey":
                text.text = CustomInputManager.IM.right.ToString();
                break;
            case "JumpKey":
                text.text = CustomInputManager.IM.jump.ToString();
                break;
            case "SprintKey":
                text.text = CustomInputManager.IM.sprint.ToString();
                break;
            case "CrouchKey":
                text.text = CustomInputManager.IM.crouch.ToString();
                break;
            case "GrabKey":
                text.text = CustomInputManager.IM.grab.ToString();
                break;
            default:
                Debug.LogWarning("O meu ganda burro, o que é isto?: " + text.name);
                break;
        }
    }

    void Update()
    {
        switch (text.name)
        {
            case "ForwardKey":
                text.text = CustomInputManager.IM.forward.ToString();
                break;
            case "BackwardKey":
                text.text = CustomInputManager.IM.backward.ToString();
                break;
            case "LeftKey":
                text.text = CustomInputManager.IM.left.ToString();
                break;
            case "RightKey":
                text.text = CustomInputManager.IM.right.ToString();
                break;
            case "JumpKey":
                text.text = CustomInputManager.IM.jump.ToString();
                break;
            case "SprintKey":
                text.text = CustomInputManager.IM.sprint.ToString();
                break;
            case "CrouchKey":
                text.text = CustomInputManager.IM.crouch.ToString();
                break;
            case "GrabKey":
                text.text = CustomInputManager.IM.grab.ToString();
                break;
            default:
                Debug.LogWarning("O meu ganda burro, o que é isto?: " + text.name);
                break;
        }
    }
}
