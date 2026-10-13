using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getVolumeMusic : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        slider.value = SoundMusic.musicVolume;
    }

    void Update()
    {
        SoundMusic.musicVolume = slider.value;
    }
}