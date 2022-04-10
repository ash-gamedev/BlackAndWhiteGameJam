using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitingTimer : MonoBehaviour
{
    [SerializeField] public Slider timerSlider;

    private bool stopTimer = false;
    public float currentTime { get; private set; }

    // Use this for initialization
    void Start()
    {
        float timerStartTime = LevelManager.TimeBeforeCustomerLeaves;

        // set values
        timerSlider.maxValue = timerStartTime;
        timerSlider.value = timerSlider.maxValue;
        currentTime = timerStartTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            stopTimer = true;
        }

        timerSlider.value = currentTime;
    }

    private string GetFormattedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        return textTime;
    }
}