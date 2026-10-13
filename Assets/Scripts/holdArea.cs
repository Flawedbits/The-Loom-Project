using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldArea : MonoBehaviour
{
    public static Transform holdArea;

    private void Awake()
    {
        holdArea = GetComponent<Transform>();
        PickUpController.holdAreaNormal = HoldArea.holdArea;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 5)
        {
            PickUpController.holdArea = holdArea;
            PickUpController.DONT = true;
        }
    }
}
