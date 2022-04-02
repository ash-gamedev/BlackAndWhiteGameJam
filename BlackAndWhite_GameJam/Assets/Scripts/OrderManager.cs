using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public Dictionary<Enumeration.Order, Sprite> OrderSprites { get; set; }

    [Header("Order Sprites")]
    [SerializeField] Sprite pearSprite;
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite hotdogSprite;

    [Header("Other Sprites")]
    [SerializeField] Sprite plateSprite;
    [SerializeField] Sprite orderBubbleSprite;

    // Use this for initialization
    void Awake()
    {
        OrderSprites = new Dictionary<Enumeration.Order, Sprite>()
            {
                { Enumeration.Order.Pear, pearSprite },
                { Enumeration.Order.Burger, burgerSprite },
                { Enumeration.Order.Hotdog, hotdogSprite }
            };
    }


    void PlateOrder()
    {

    }
}