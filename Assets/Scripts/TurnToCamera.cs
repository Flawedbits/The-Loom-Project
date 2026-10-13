using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnToCamera : MonoBehaviour
{

    public Camera camera;
    public GameObject player;
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 20)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
    }
}
