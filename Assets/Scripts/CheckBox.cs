using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        if (TriggerBox.boxhit)
        {
            Instantiate(box, transform);
        }
    }
}
