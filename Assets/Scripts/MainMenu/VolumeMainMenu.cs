using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMainMenu : MonoBehaviour
{
    public SettingsMainMenu settingsMenuScript;
    public MainMenu mainMenuScript;
    public SoundMusic soundMusicScript;

    // Update is called once per frame
    void Update()
    {
        if (SettingsMainMenu.volumeOpen)
        {
            CollisionSounds.volume = soundEffect.volume;

            if (Input.GetButtonDown("Pause"))
            {
                Back();
            }
        }
    }

    public void Back()
    {
        SaveData.saveVolumeToJson(SoundMusic.musicVolume, soundEffect.volume);
        settingsMenuScript.enabled = true;
        mainMenuScript.settingsMenuUI.SetActive(true);
        settingsMenuScript.volumeMenuUI.SetActive(false);
        SettingsMainMenu.volumeOpen = false;
    }
}
