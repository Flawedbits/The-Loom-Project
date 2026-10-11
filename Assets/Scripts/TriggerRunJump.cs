using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerRunJump : MonoBehaviour
{
    public GameObject player;
    float totTime;
    float timeBeforePause = 5f;
    bool checkRunJump = false;
    public Text text;

    // Update is called once per frame
    void Update()
    {
        totTime += Time.deltaTime;

        //  Check to see if the current mouse position input is equivalent to updateMousePosition from the previous update
        //  If they are equivalent, this means that the user hasn't moved the mouse
        if (Input.GetKey(CustomInputManager.IM.sprint) || Input.GetKey(CustomInputManager.IM.jump))
        {
            totTime = 0;
            checkRunJump = true;
        }
        else
        {
            if (totTime >= timeBeforePause)
            {
                checkRunJump = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            if (checkRunJump)
            {
                if (PickUpController.beingHeld)
                {
                    PickUpController.DropObject();
                }
                player.transform.position = new Vector3(0, 1.31f, 18);
                text.text = "The wise don't rush to finish a thought";
                StartCoroutine(fadeInOut.FadeTextInAndOut(text, 1f, 1f));
            }
        }
    }
}
