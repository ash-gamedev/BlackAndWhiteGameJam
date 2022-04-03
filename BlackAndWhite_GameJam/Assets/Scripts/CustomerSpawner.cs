using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Spawner")]
    [SerializeField] GameObject customer = null;
    [SerializeField] float timeBetweenCustomerSpawns = 10f;
    [SerializeField] float customerTimeVariance = 1f;

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
        while (true)
        {
            // find seat with no customer
            Seat seat = seats.FirstOrDefault(x => x.GetCustomer() == false);

            // if available seat, assign customer to chair
            if(seat != null)
            {
                seat.SetCustomer(customer);
            }
            
            // wait to spawn next customer
            yield return new WaitForSeconds(timeBetweenCustomerSpawns * customerTimeVariance);
        }
    }
}