using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerOutOfBounds : MonoBehaviour
{
    public Text text;
    public GameObject box;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "box")
        {
            text.text = "Don't do that";
            StartCoroutine(fadeInOut.FadeTextInAndOut(text, 1f, 1f));
            box.transform.position = new Vector3(0, 3.7f, 0);
        }
        else if (other.gameObject.name == "PlayerCollider")
        {
            if (PickUpController.beingHeld)
            {
                PickUpController.DropObject();
            }
            player.transform.position = new Vector3(0, 1.31f, 0);
            text.text = "I honestly don't even know how you did that";
            StartCoroutine(fadeInOut.FadeTextInAndOut(text, 1f, 3f));
        }
    }
}