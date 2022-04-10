using System.Collections;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    
    static int tips = 0;
    static int maxTip = 8;
    static int pointsForCorrectOrder = 20;
    static int pointsForRecplacementOrder = 5;
    static int pointsForBrokenPlate = 5;
    static int numberOfReplacementOrders = 0;
    static int numberOfCorrectOrders = 0;
    static int numberOfPlatesBroken = 0;
    static ScoreKeeper instance;

    // properties
    public static int Score
    {
        get
        {
            int score = 0;
            score += (pointsForCorrectOrder * numberOfCorrectOrders);
            score += tips;
            score -= (pointsForBrokenPlate * numberOfPlatesBroken);
            score -= (pointsForRecplacementOrder * numberOfReplacementOrders); 

            Mathf.Clamp(score, int.MinValue, int.MaxValue);

            return score;
        }
    }

    public static float ScorePercentage
    {
        get
        {
            return (float)Score / (float)MaxScore; ;
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

    public static bool OneStar
    {
        get
        {
            return ScorePercentage > 0.3f;
        }
    }

    public static bool TwoStar
    {
        get
        {
            return ScorePercentage > 0.6f;
        }
    }

    public static bool ThreeStar
    {
        get
        {
            return ScorePercentage > 0.9f;
        }
    }

    public static int MaxScore
    {
        get
        {
            return (LevelManager.NumberOfCustomers * ScoreKeeper.MaxTip) + (LevelManager.NumberOfCustomers * pointsForCorrectOrder);
        }
    }

    public static int NumberOfCorrectOrders
    {
        get
        {
            return numberOfCorrectOrders;
        }
    }

    public static int NumberOfReplacementOrders
    {
        get
        {
            return numberOfReplacementOrders;
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

    public static int PointsForReplacementOrder
    {
        get
        {
            return pointsForRecplacementOrder;
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
        instance = this;
        ResetScore();
    }
    #endregion

    #region public functions
    public static void OrderCompleted()
    {
        numberOfCorrectOrders++;
    }
    public static void OrderReplaced()
    {
        numberOfReplacementOrders++;
    }
    public static void PlateBroken()
    {
        numberOfPlatesBroken++;
    }
    public static int AddTip(float timeOrderReceived)
    {
        float waitTimePercentage = (LevelManager.TimeBeforeCustomerLeaves - timeOrderReceived)/LevelManager.TimeBeforeCustomerLeaves;
               
        int tip = 1;

        // waiting less then 30% of time
        if(waitTimePercentage < 0.3f)
        {
            tip = maxTip;
        }
        else if(waitTimePercentage >= 0.3f && waitTimePercentage < 0.6f)
        {
            tip = (int)(maxTip*0.5);
        }
        else if(waitTimePercentage >= 0.6f && waitTimePercentage <= 0.9f)
        {
            tip = (int)(maxTip * 0.25);
        }

        tips += tip;

        return tip;
    }
    public static void ResetScore()
    {
        tips = 0;
        numberOfCorrectOrders = 0;
        numberOfPlatesBroken = 0;
        numberOfReplacementOrders = 0;
    }
    #endregion

}