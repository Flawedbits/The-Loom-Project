using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusic : MonoBehaviour
{
    float musicTime = 73.221f;
    public static float musicVolume = 0.5f;
    public static bool playing1 = false;
    public static bool playing2 = false;
    public AudioSource ambientAudio1;
    public AudioSource ambientAudio2;

    public void Awake()
    {
        ambientAudio1.Play();
        playing1 = true;
    }

    void Start()
    {
        string filePath = Application.persistentDataPath + "/VolumeData.json";
        if (System.IO.File.Exists(filePath))
        {
            musicVolume = SaveData.loadVolumeFromJson()[0];
        }

        ambientAudio1.volume = musicVolume;
        ambientAudio2.volume = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        ambientAudio1.volume = SoundMusic.musicVolume;
        ambientAudio2.volume = SoundMusic.musicVolume;
        if (ambientAudio1.time >= musicTime && playing1)
        {
            StartCoroutine(fadeInOut.FadeAudioOutPlayNext(ambientAudio1, ambientAudio2, 5f));
            playing1 = false;
            playing2 = true;
        }
        if (ambientAudio2.time >= musicTime && playing2)
        {
            StartCoroutine(fadeInOut.FadeAudioOutPlayNext(ambientAudio2, ambientAudio1, 5f));
            playing2 = false;
            playing1 = true;
        }
        Debug.Log(ambientAudio1.time);
        Debug.Log(ambientAudio2.time);
        if (MainMenu.gameStarted)
        {
            StartCoroutine(fadeInOut.FadeAudioOut(100f));
        }
    }
}
