using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Dynamic;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float startTime;
    public float pausedTime;
    private bool isTimerRunning = false;

    private SaveData saveData;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
        saveData = FindObjectOfType<SaveData>();
        timerText.text = "00:00.000";
        isTimerRunning = false;
    }

    void LateUpdate()
    {
        if (isTimerRunning)
        {
            float elapsedTime = Time.time - startTime;

            if (pausedTime > 0)
            {
                elapsedTime -= pausedTime;
                pausedTime = 0;
            }

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

            timerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            startTime = Time.time;
            isTimerRunning = true;
        }
    }

    public void StopTimer()
    {
        if (isTimerRunning)
        {
            pausedTime = Time.time - startTime;
            isTimerRunning = false;
        }
    }

    public void ResumeTimer()
    {
        if (!isTimerRunning && pausedTime > 0)
        {
            startTime = Time.time - pausedTime;
            isTimerRunning = true;
        }
    }

    public void ResetTimer()
    {

        timerText.text = "00:00.000";
        isTimerRunning = false;
        pausedTime = 0;
    }
}