using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Spawner")]
    [SerializeField] GameObject customer = null;

    List<Seat> seats = new List<Seat>();


    // Use this for initialization
    void Start()
    {
        GameObject[] chairObjects = GameObject.FindGameObjectsWithTag(EnumTags.Chair.ToString());
        foreach (GameObject chairObject in chairObjects)
        {
            seats.Add(chairObject.GetComponent<Seat>());
        }

        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        int numberOfCustomers = LevelManager.NumberOfCustomers;
        int customerNumberWhenSpeedIncreases = Mathf.FloorToInt((float)(numberOfCustomers) * 0.7f);
        int customerCount = 0;

        float timeBetweenCustomerSpawns = LevelManager.TimeBetweenCustomerSpawns;

        while (customerCount < numberOfCustomers)
        {            
            // find random seat with no customer
            System.Random rnd = new System.Random();
            Seat seat = seats.Where(x => x.GetCustomer() == false)
                        .OrderBy(c => rnd.Next())
                        .FirstOrDefault();

            // if available seat, assign customer to chair
            if(seat != null)
            {
                seat.SetCustomer(customer);
                customerCount++;
            }

            // increase speed
            if (customerCount == customerNumberWhenSpeedIncreases)
            {
                timeBetweenCustomerSpawns -= (timeBetweenCustomerSpawns * LevelManager.CustomerSpeedIncreasePercentage);
                Debug.Log("customerCount: " + customerCount);
                Debug.Log("timeBetweenCustomerSpawns: " + timeBetweenCustomerSpawns);
            }

            // wait to spawn next customer
            yield return new WaitForSeconds(timeBetweenCustomerSpawns * LevelManager.CustomerTimeVariance);
        }

        // wait until all customers have left
        yield return new WaitUntil(() => FindObjectsOfType<Customer>().Length == 0);


        LevelManager.OnLevelComplete();
    }
}