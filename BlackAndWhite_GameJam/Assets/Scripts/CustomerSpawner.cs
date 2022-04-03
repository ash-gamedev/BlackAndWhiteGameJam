using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CustomerSpawner : MonoBehaviour
    {
        [Header("Customer Spawner")]
        [SerializeField] GameObject customer;
        [SerializeField] float timeBetweenCustomerSpawns = 10f;
        
        [Header("Seat Objects")]
        [SerializeField] List<GameObject> topSeats;
        [SerializeField] List<GameObject> bottomSeats;
        [SerializeField] List<GameObject> leftSeats;
        [SerializeField] List<GameObject> rightSeats;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}