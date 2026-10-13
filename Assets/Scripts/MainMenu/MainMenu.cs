using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUi;
    public GameObject settingsMenuUI;
    public GameObject controlsUi;
    public TMP_Text[] text;
    public Image[] image;

    public static bool settingsOpen = false;
    public static bool gameStarted = false;

    public void Play()
    {
        StartCoroutine(fadeInOut.FadeImageIn(image[0], 1f));

        gameStarted = true;

        //Hide buttons
        StartCoroutine(fadeInOut.FadeImageOut(image[1], 1f));
        StartCoroutine(fadeInOut.FadeImageOut(image[2], 1f));
        StartCoroutine(fadeInOut.FadeImageOut(image[3], 1f));
        StartCoroutine(fadeInOut.FadeImageOut(image[4], 1f));
        StartCoroutine(fadeInOut.FadeTextProOut(text[0], 1f));
        StartCoroutine(fadeInOut.FadeTextProOut(text[1], 1f));
        StartCoroutine(fadeInOut.FadeTextProOut(text[2], 1f));
        StartCoroutine(fadeInOut.FadeTextProOut(text[3], 1f));

        controlsUi.SetActive(true);

        StartCoroutine(fadeInOut.FadeImageInAndOut(image[5], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeImageInAndOut(image[6], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeImageInAndOut(image[7], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeImageInAndOut(image[8], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeImageInAndOut(image[9], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeTextProInAndOut(text[4], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeTextProInAndOut(text[5], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeTextProInAndOut(text[6], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeTextProInAndOut(text[7], 1f, 2f, 1f));
        StartCoroutine(fadeInOut.FadeTextProInAndOut(text[8], 1f, 2f, 1f));

        StartCoroutine(fadeInOut.LoadScene(5f));
    }

    public void Choose()
    {
        
    }

    public void Settings()
    {
        mainMenuUi.SetActive(false);
        settingsMenuUI.SetActive(true);
        settingsOpen = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
