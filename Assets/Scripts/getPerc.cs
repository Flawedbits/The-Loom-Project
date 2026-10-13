using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class getPerc : MonoBehaviour
{
    public Slider slider;
    public TMP_Text text;

    void Update()
    {
        int value = (int)Math.Round(slider.value * 100);
        text.text = value.ToString() + "%";
    }
}
