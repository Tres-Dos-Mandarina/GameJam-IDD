using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public GameConfig config;
    public void SaveToJson(bool _speedrun, bool _audio, bool _kenkri)
    {
        config.speedrun = _speedrun;
        config.audio = _audio;
        config.kenkri = _kenkri;
        string configSettings = JsonUtility.ToJson(config);
        string filePath = Application.persistentDataPath + "/oriol_gilipollas.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, configSettings);
    }
    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/oriol_gilipollas.json";
        string settings = System.IO.File.ReadAllText(filePath);

        config = JsonUtility.FromJson<GameConfig>(settings);
    }
}

[System.Serializable]
public class GameConfig
{
    public bool speedrun = false;
    public bool audio = false;
    public bool kenkri = false;
}
