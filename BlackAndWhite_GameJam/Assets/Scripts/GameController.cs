using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController: MonoBehaviour
{
    [SerializeField] float easyModifier = 1.25f;
    [SerializeField] float normalModifier = 1f;
    [SerializeField] float hardModifier = 0.75f;

    // static persists through all instances of a class
    static GameController instance;
    public EnumDifficulty DifficultySetting { get; private set; }
    public Dictionary<EnumDifficulty, float> AdjustmentsFromDifficulty { get; private set; }
    public float AdjustmentFromDifficulty
    {
        get
        {
            return AdjustmentsFromDifficulty[DifficultySetting];
        }
    }
    
    private void Awake()
    {
        ManageSingleton();

        AdjustmentsFromDifficulty = new Dictionary<EnumDifficulty, float>()
        {
            { EnumDifficulty.Easy, easyModifier },
            { EnumDifficulty.Normal, normalModifier },
            { EnumDifficulty.Hard, hardModifier }
        };

        DifficultySetting = EnumDifficulty.Easy;
    }

    public void SetDifficultySetting(EnumDifficulty difficulty)
    {
        DifficultySetting = difficulty;
        Debug.Log("Setting difficulty to: " + difficulty);
    }

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

}