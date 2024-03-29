﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController: MonoBehaviour
{
    float easyModifier = 1.25f;
    float normalModifier = 1f;
    float hardModifier = 0.85f;

    int easyMaxCustomers = 3;
    int normalMaxCustomers = 3;
    int hardMaxCustomers = 4;

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
    public int MaxNumberOfCustomers
    {
        get
        {
            if (DifficultySetting == EnumDifficulty.Easy)
                return easyMaxCustomers;
            else if (DifficultySetting == EnumDifficulty.Normal)
                return normalMaxCustomers;
            else // hard
                return hardMaxCustomers;
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