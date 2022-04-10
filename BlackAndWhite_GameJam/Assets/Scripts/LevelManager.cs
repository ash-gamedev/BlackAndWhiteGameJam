using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // level customizations
    [SerializeField] int numberOfCustomers = 20;
    [SerializeField] float timeBetweenCustomerSpawns = 5f;
    [SerializeField] float timeBeforeCustomerLeaves = 30f;
    [SerializeField] float customerTimeVariance = 1f;
    [SerializeField] float customerSpeedIncreasePercentage = 0.2f;
    [SerializeField] float conveyorSpeed = 1.5f;

    private bool levelComplete = false;

    // properties
    public static bool LevelComplete
    {
        get
        {
            return instance.levelComplete;
        }
    } 
    public static int NumberOfCustomers
    {
        get
        {
            return instance.numberOfCustomers;
        }
    } 
    public static float TimeBetweenCustomerSpawns
    {
        get
        {
            return instance.timeBetweenCustomerSpawns;
        }
    }
    public static float CustomerTimeVariance
    {
        get
        {
            return instance.customerTimeVariance;
        }
    }
    public static float ConveyorSpeed
    {
        get
        {
            return instance.conveyorSpeed;
        }
    }
    public static float TimeBeforeCustomerLeaves
    {
        get
        {
            return instance.timeBeforeCustomerLeaves;
        }
    }
    public static float CustomerSpeedIncreasePercentage
    {
        get
        {
            return instance.customerSpeedIncreasePercentage;
        }
    }

    // static persists through all instances of a class
    static LevelManager instance;

    #region Awake, Start, Update
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public static void OnLevelComplete()
    {
        instance.levelComplete = true;

        // play sound & update next level button
        if (ScoreKeeper.OneStar)
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.LevelWinSound);
        else
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.LevelLostSound);

        FindObjectOfType<UIManager>().UpdateLevelCompleteUI();
    }
}