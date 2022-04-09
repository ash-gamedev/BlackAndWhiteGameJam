using System.Collections;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    
    static int score = 0;
    static int tips = 0;
    static int maxTip = 8;
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

    public static int Tips
    {
        get
        {
            return tips;
        }
    }

    public static int MaxTip
    {
        get
        {
            return maxTip;
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
    public static void OrderCompleted()
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
    public static int AddTip(float timeOrderReceived)
    {
        float waitTimePercentage = (LevelManager.TimeBeforeCustomerLeaves - timeOrderReceived)/LevelManager.TimeBeforeCustomerLeaves;
               
        int tip = 1;

        // waiting less then 60% of time
        if(waitTimePercentage < 0.6f)
        {
            tip = maxTip;
        }
        else if(waitTimePercentage >= 0.6f && waitTimePercentage < 0.7f)
        {
            tip = (int)(maxTip*0.5);
        }
        else if(waitTimePercentage >= 0.7f && waitTimePercentage <= 0.8f)
        {
            tip = (int)(maxTip * 0.25);
        }

        Debug.Log("timeOrderReceived " + timeOrderReceived + " waitPercentage: " + waitTimePercentage + " tip: " + tip);

        score += tip;
        tips += tip;

        return tip;
    }
    public static void ResetScore()
    {
        score = 0;
        tips = 0;
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