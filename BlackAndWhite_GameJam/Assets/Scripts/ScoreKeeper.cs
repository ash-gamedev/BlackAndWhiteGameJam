using System.Collections;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    
    static int score = 0;
    static int pointsForCorrectOrder = 20;
    static int pointsForBrokenPlate = 5;
    static int numberOfCorrectOrders = 0;
    static int numberOfPlatesBroken = 0;
    static ScoreKeeper instance;

    // properties
    public static int Score
    {
        get
        {
            return score;
        }
    }

    public static int NumberOfCorrectOrders
    {
        get
        {
            return numberOfCorrectOrders;
        }
    }

    public static int NumberOfPlatesBroken
    {
        get
        {
            return numberOfPlatesBroken;
        }
    }

    public static int PointsForCorrectOrder
    {
        get
        {
            return pointsForCorrectOrder;
        }
    }

    public static int PointsForBrokenPlate
    {
        get
        {
            return pointsForBrokenPlate;
        }
    }
    #region Awake

    private void Awake()
    {
        ManageSingleton();
    }
    #endregion

    #region public functions
    public static void AddToScore()
    {
        score += pointsForCorrectOrder;
        Mathf.Clamp(score, 0, int.MaxValue);

        numberOfCorrectOrders++;
    }
    public static void PlateBroken()
    {
        score -= pointsForBrokenPlate;
        Mathf.Clamp(score, 0, int.MaxValue);

        numberOfPlatesBroken++;
    }
    public static void ResetScore()
    {
        score = 0;
        numberOfCorrectOrders = 0;
        numberOfPlatesBroken = 0;
    }
    #endregion

    #region private functions
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}