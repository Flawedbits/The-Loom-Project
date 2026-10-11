using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMenu : MonoBehaviour
{
    public SettingsMenu settingsMenuScript;
    public PauseMenu pauseMenuScript;

    // Update is called once per frame
    void Update()
    {
        if (SettingsMenu.volumeOpen)
        {
            settingsMenuScript.enabled = false;
            pauseMenuScript.ambientAudio.volume = SoundMusic.musicVolume;
            CollisionSounds.volume = soundEffect.volume;

            if (Input.GetButtonDown("Pause"))
            {
                Back();
                if (pauseMenuScript.ambientAudio.volume > 0.25f)
                {
                    pauseMenuScript.ambientAudio.volume = 0.25f;
                }
            }
        }
    }

    public void Back()
    {
        SaveData.saveVolumeToJson(SoundMusic.musicVolume, soundEffect.volume);
        settingsMenuScript.enabled = true;
        pauseMenuScript.settingsMenuUI.SetActive(true);
        settingsMenuScript.volumeMenuUI.SetActive(false);
        SettingsMenu.volumeOpen = false;
    }
}
