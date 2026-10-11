using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class fadeInOut : MonoBehaviour
{
    public static IEnumerator FadeTextInAndOut(Text text, float fadeTime, float waitTime)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeTextProInAndOut(TMP_Text text, float fadeTime, float waitTime)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeTextProInAndOut(TMP_Text text, float fadeTime, float waitTime, float waitBefore)
    {
        yield return new WaitForSeconds(waitBefore);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeTextProIn(TMP_Text text, float fadeTime)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeTextProOut(TMP_Text text, float fadeTime)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeImageInAndOut(Image image, float fadeTime, float waitTime)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeImageInAndOut(Image image, float fadeTime, float waitTime, float waitBefore)
    {
        yield return new WaitForSeconds(waitBefore);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeImageIn(Image image, float fadeTime)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeImageOut(Image image, float fadeTime)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeAudioOut(AudioSource audio, float fadeTime)
    {
        while (audio.volume > 0.0f)
        {
            audio.volume = audio.volume - (Time.deltaTime / fadeTime);
            yield return null;
        }
    }

    public static IEnumerator FadeAudioOut(float fadeTime)
    {
        while (SoundMusic.musicVolume > 0.0f)
        {
            SoundMusic.musicVolume = SoundMusic.musicVolume - (Time.deltaTime / fadeTime);
            yield return null;
        }
    }

    public static IEnumerator FadeAudioOutPlayNext(AudioSource audio1, AudioSource audio2, float fadeTime)
    {
        audio2.volume = SoundMusic.musicVolume;
        audio2.Play();
        while (audio1.volume > 0.0f)
        {
            audio1.volume = audio1.volume - (Time.deltaTime / fadeTime);
            yield return null;
        }
        audio1.Stop();
    }

    public static IEnumerator LoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        string filePath = Application.persistentDataPath + "/VolumeData.json";
        if (System.IO.File.Exists(filePath))
        {
            SoundMusic.musicVolume = SaveData.loadVolumeFromJson()[0];
        }
        MainMenu.gameStarted = false;
        SceneManager.LoadScene("Scenes/Mind/Mind");
    }

    public static IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
