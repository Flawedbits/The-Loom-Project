using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public PlayerAiming PlayerCam;
    public GameObject GUIPanel;
    public GameObject pauseMenuUi;
    public GameObject settingsMenuUI;
    public AudioSource ambientAudio;

    public static float musicVolume;
    public static bool gameispaused = false;
    public static bool settingsOpen = false;

    void Start()
    {
        musicVolume = SoundMusic.musicVolume;
        PlayerCam.enabled = true;
        ambientAudio.volume = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        musicVolume = SoundMusic.musicVolume;
        if (Input.GetButtonDown("Pause"))
        {
            if (gameispaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        GUIPanel.SetActive(true);
        ambientAudio.volume = musicVolume;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCam.enabled = true;
        gameispaused = false;
    }

    void Pause()
    {
        pauseMenuUi.SetActive(true);
        GUIPanel.SetActive(false);
        if (ambientAudio.volume > 0.25f)
        {
            ambientAudio.volume = 0.25f;
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerCam.enabled = false;
        gameispaused = true;
    }

    public void OpenSettings()
    {
        pauseMenuUi.SetActive(false);
        settingsMenuUI.SetActive(true);
        settingsOpen = true;
    }

    public void ReturnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
