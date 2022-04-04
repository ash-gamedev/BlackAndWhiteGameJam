using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public Dictionary<EnumOrder, Sprite> OrderSprites { get; set; }

    [Header("Order Sprites")]
    [SerializeField] Sprite pearSprite;
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite hotdogSprite;

    [SerializeField] GameObject platedOrderPrefab;
    [SerializeField] float timeBetweenOrderSpawns = 5f;

    System.Random random;

    // Use this for initialization
    void Awake()
    {
        OrderSprites = new Dictionary<EnumOrder, Sprite>()
            {
                { EnumOrder.Pear, pearSprite },
                { EnumOrder.Burger, burgerSprite },
                { EnumOrder.Hotdog, hotdogSprite }
            };
    }

    private void Start()
    {
        //StartCoroutine(OrderSpawning());
    }

    private IEnumerator OrderSpawning()
    {
        while (true)
        {
            // create random order
            Array orders = Enum.GetValues(typeof(EnumOrder));
            random = new System.Random();
            EnumOrder order = (EnumOrder)orders.GetValue(random.Next(orders.Length));

            // spawn order
            SpawnOrder(order);

            yield return new WaitForSeconds(timeBetweenOrderSpawns);
        }
    }

    public void SpawnOrder(EnumOrder order)
    {
        GameObject platedOrderInstance = platedOrderPrefab;
        platedOrderInstance.GetComponent<Order>().order = order;
        Instantiate(platedOrderInstance,  // what object to instantiate
                        transform.position, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
    }
}