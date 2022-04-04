using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitingTimer : MonoBehaviour
{
    [SerializeField] float timerStartTime = 100f;
    //[SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider timerSliderPrefab;

    Canvas uiCanvas;
    Slider timerSlider;
    private bool stopTimer;
    private float currentTime;

    // Use this for initialization
    void Start()
    {
        uiCanvas = FindObjectOfType<Canvas>();
        AddTimerSliderToUI();
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

        if (stopTimer == false)
        {
            //timerText.text = textTime;
            timerSlider.value = currentTime;
        }
    }

    private void AddTimerSliderToUI()
    {
        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = uiCanvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y)));

        Debug.Log(ViewportPosition + ", " + WorldObject_ScreenPosition);

        // instantiate slider
        timerSlider = Instantiate(timerSliderPrefab, WorldObject_ScreenPosition, Quaternion.identity, uiCanvas.transform);

        // set values
        timerSlider.maxValue = timerStartTime;
        timerSlider.value = 0;
    }

    private string GetFormattedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        return textTime;
    }
}