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
    [SerializeField] TextMeshProUGUI levelCompleteTitleText;
    [SerializeField] TextMeshProUGUI levelCompleteResultsText;
    [SerializeField] TextMeshProUGUI levelCompleteNextLevelButton;
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
        Time.timeScale = 0f; //freezes fame

        
        // text
        string levelCompleteTextBlock =
            $"{ScoreKeeper.NumberOfCorrectOrders} x ${ScoreKeeper.PointsForCorrectOrder} = ${ScoreKeeper.NumberOfCorrectOrders*ScoreKeeper.PointsForCorrectOrder}\n" +
            $"{ScoreKeeper.NumberOfPlatesBroken} x ${ScoreKeeper.PointsForBrokenPlate} = -${ScoreKeeper.NumberOfPlatesBroken*ScoreKeeper.PointsForBrokenPlate}\n" +
            "\n" +
            $"+ ${ScoreKeeper.Tips} (tips)\n" +
            $"${ScoreKeeper.Score}";

        levelCompleteResultsText.text = levelCompleteTextBlock;

        // stars
        float scorePercentage = ScoreKeeper.ScorePercentage;
        if (starImages?.Count == 3)
        {
            Debug.Log("scorePercentage: " + scorePercentage);

            starImages[0].sprite = ScoreKeeper.OneStar ? fullStarSprite : emptyStartSprite;
            starImages[1].sprite = ScoreKeeper.TwoStar ? fullStarSprite : emptyStartSprite;
            starImages[2].sprite = ScoreKeeper.ThreeStar ? fullStarSprite : emptyStartSprite;
        }

        // play sound & update next level button
        if (ScoreKeeper.OneStar)
        {
            levelCompleteTitleText.text = "Level Completed";
            levelCompleteNextLevelButton.text = "Next Level";
        }
        else
        {
            levelCompleteTitleText.text = "Level Failed";
            levelCompleteNextLevelButton.text = "Retry Level";
        }
            
        // show panel
        levelCompletePanel.SetActive(true);
    }


    public GameObject InstantiateObjectOnUi(Vector3 objectSpawnPos, GameObject objectToInstantiate)
    {
        //first you need the RectTransform component of your canvas
        Canvas uiCanvas = FindObjectOfType<Canvas>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(objectSpawnPos);
        float h = Screen.height;
        float w = Screen.width;
        float x = ViewportPosition.x - (w / 2);
        float y = ViewportPosition.y - (h / 2);
        float s = uiCanvas.scaleFactor;
        Vector2 objectSpawnPosUi = new Vector2(x, y) / s;

        GameObject objectInstance = Instantiate(objectToInstantiate, objectSpawnPosUi, Quaternion.identity, uiCanvas.transform);
        objectInstance.transform.SetAsFirstSibling(); // order in hierarchy to top (so it appears under menus, etc.)
        objectInstance.GetComponent<RectTransform>().anchoredPosition = objectSpawnPosUi;

        return objectInstance;
    }
}