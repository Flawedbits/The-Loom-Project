using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static void saveVolumeToJson(float volumeMusic, float volumeSE){
        Volume volume = new Volume(volumeMusic, volumeSE);
        string volumeData = JsonUtility.ToJson(volume);
        string filePath = Application.persistentDataPath + "/VolumeData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, volumeData);
        Debug.Log("Saved");
    }

    public static float[] loadVolumeFromJson(){
        string filePath = Application.persistentDataPath + "/VolumeData.json";
        string volumeData = System.IO.File.ReadAllText(filePath);

        Volume volume = JsonUtility.FromJson<Volume>(volumeData);
        return new float[]{volume.volumeMusic, volume.volumeSE};
    }

    public static void saveControlsToJson(KeyCode forward, KeyCode backward, KeyCode left, KeyCode right, KeyCode jump, KeyCode sprint, KeyCode crouch, KeyCode grab){
        Controls controls = new Controls(forward, backward, left, right, jump, sprint, crouch, grab);
        string controlsData = JsonUtility.ToJson(controls);
        string filePath = Application.persistentDataPath + "/ControlsData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, controlsData);
        Debug.Log("Saved");
    }

    public static KeyCode[] loadControlsFromJson()
    {
        string filePath = Application.persistentDataPath + "/ControlsData.json";
        string controlsData = System.IO.File.ReadAllText(filePath);

        Controls controls = JsonUtility.FromJson<Controls>(controlsData);
        return new KeyCode[] { controls.forward, controls.backward, controls.left, controls.right, controls.jump, controls.sprint, controls.crouch, controls.grab};
    }
}

public class Volume
{
    public float volumeMusic;
    public float volumeSE;
    
    public Volume(float volumeMusic, float volumeSE){
        this.volumeMusic = volumeMusic;
        this.volumeSE = volumeSE;
    }
}

public class Controls
{
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode grab;
    
    public Controls(KeyCode forward, KeyCode backward, KeyCode left, KeyCode right, KeyCode jump, KeyCode sprint, KeyCode crouch, KeyCode grab){
        this.forward = forward;
        this.backward = backward;
        this.left = left;
        this.right = right;
        this.jump = jump;
        this.sprint = sprint;
        this.crouch = crouch;
        this.grab = grab;
    }
}