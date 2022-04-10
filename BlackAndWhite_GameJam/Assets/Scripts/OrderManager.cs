using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public Dictionary<EnumOrder, Sprite> OrderSprites { get; set; }

    [Header("Order Sprites")]
    [SerializeField] Sprite coffeeSprite;
    [SerializeField] Sprite croissantSprite;
    [SerializeField] Sprite donutSprite;
    [SerializeField] Sprite cookieSprite;
    [SerializeField] Sprite sandwichSprite;

    [Header("Other")]
    [SerializeField] GameObject platedOrderPrefab;
    [SerializeField] float timeBetweenOrderSpawns = 0.5f;

    [Header("New Order")]
    [SerializeField] GameObject newOrderPlacedObject;
    GameObject newOrderPlaceObjectInstance = null;

    System.Random random;
    private Queue<EnumOrder> OrderQueue;

    // Use this for initialization
    void Awake()
    {
        OrderSprites = new Dictionary<EnumOrder, Sprite>()
            {
                { EnumOrder.Coffee, coffeeSprite },
                { EnumOrder.Croissant, croissantSprite },
                { EnumOrder.Donut, donutSprite },
                { EnumOrder.Cookie, cookieSprite },
                { EnumOrder.Sandwich, sandwichSprite }
            };
    }

    private void Start()
    {
        OrderQueue = new Queue<EnumOrder>();
        StartCoroutine(OrderSpawning());
    }

    private IEnumerator OrderSpawning()
    {
        while (true)
        {
            // wait for items in queue to spawn
            yield return new WaitUntil(() => OrderQueue.Count > 0);

            // spawn order
            SpawnOrder();

            yield return new WaitForSeconds(timeBetweenOrderSpawns);
        }
    }

    public void AddOrderToQueue(EnumOrder order, bool replacementOrder = false)
    {
        OrderQueue.Enqueue(order);

        if (replacementOrder)
            NewOrderOnUI(order);
    }

    public void SpawnOrder()
    {
        EnumOrder order = OrderQueue.Dequeue();

        GameObject platedOrderInstance = platedOrderPrefab;
        platedOrderInstance.GetComponent<Order>().order = order;
        Instantiate(platedOrderInstance,  // what object to instantiate
                        transform.position, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
    }

    private void NewOrderOnUI(EnumOrder order)
    {
        Destroy(newOrderPlaceObjectInstance); // destroy instance if exists
        Vector2 orderSpawnerPosition = transform.position;
        newOrderPlaceObjectInstance = FindObjectOfType<UIManager>().InstantiateObjectOnUi(orderSpawnerPosition, newOrderPlacedObject);

        // update sprite
        newOrderPlaceObjectInstance.GetComponentInChildren<Image>().sprite = OrderSprites[order];
    }
}