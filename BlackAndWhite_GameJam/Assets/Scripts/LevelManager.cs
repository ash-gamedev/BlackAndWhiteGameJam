using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // level customizations
    [SerializeField] int numberOfCustomers = 20;
    [SerializeField] float timeBetweenCustomerSpawns = 5f;
    [SerializeField] float timeBeforeCustomerLeaves = 30f;
    [SerializeField] float customerTimeVariance = 1f;
    [SerializeField] float conveyorSpeed = 1.5f;

    // properties
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
    
    // static persists through all instances of a class
    static LevelManager instance;

    #region Awake, Start, Update
    private void Awake()
    {
        ManageSingleton();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

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