using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Core UI")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Level Complete UI")]
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TextMeshProUGUI levelCompleteText;
    [SerializeField] List<Image> starImages;
    [SerializeField] Sprite emptyStartSprite;
    [SerializeField] Sprite fullStarSprite;

    void Update()
    {
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = (ScoreKeeper.Score).ToString();
    }

    public void UpdateLevelCompleteUI()
    {
        int numberOfCustomers = LevelManager.NumberOfCustomers;
        int numberOfCorrectOrders = ScoreKeeper.NumberOfCorrectOrders;
        int numberOfIncorrectOrders = numberOfCustomers - numberOfCorrectOrders;
        int totalScore = ScoreKeeper.Score;

        // text
        string levelCompleteTextBlock =
            $"{numberOfCorrectOrders} \n" +
            $"{numberOfIncorrectOrders} \n" +
            "\n" +
            $"{totalScore}";

        levelCompleteText.text = levelCompleteTextBlock;

        // stars
        if(starImages?.Count == 3)
        {
            float levelPercentage = (float)numberOfCorrectOrders / (float)numberOfCustomers;

            starImages[0].sprite = levelPercentage >= 0.25f ? fullStarSprite : emptyStartSprite;
            starImages[1].sprite = levelPercentage >= 0.5f ? fullStarSprite : emptyStartSprite;
            starImages[2].sprite = levelPercentage >= 0.75f ? fullStarSprite : emptyStartSprite;
        }

        // show panel
        levelCompletePanel.SetActive(true);
    }
}