using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetVolumeSE : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        slider.value = soundEffect.volume;
    }

    void Update()
    {
        soundEffect.volume = slider.value;
    }
}
