using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyForMainMenu : MonoBehaviour
{
    private SaveData saveData;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        saveData = FindObjectOfType<SaveData>();
        saveData.config.time = 0f;
        saveData.SaveToJson(saveData.config.speedrun, saveData.config.audio, saveData.config.kenkri);
    }
}
