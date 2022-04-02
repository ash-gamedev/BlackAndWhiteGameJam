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

    [Header("Other Sprites")]
    [SerializeField] Sprite plateSprite;
    [SerializeField] Sprite orderBubbleSprite;

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


    void PlateOrder()
    {
        
    }
}