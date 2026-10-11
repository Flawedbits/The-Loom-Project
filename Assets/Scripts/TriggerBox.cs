using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public static bool boxhit = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "box")
        {
            boxhit = true;
        }
    }
}
