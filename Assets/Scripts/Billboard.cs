
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null)
            return;

        transform.forward = targetCamera.transform.forward;
    }
}
