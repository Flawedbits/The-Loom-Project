using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffect : MonoBehaviour
{
    public static float volume = 0.5f;
    void Start()
    {
        string filePath = Application.persistentDataPath + "/VolumeData.json";
        if (System.IO.File.Exists(filePath))
        {
            volume = SaveData.loadVolumeFromJson()[1];
        }
    }
}
