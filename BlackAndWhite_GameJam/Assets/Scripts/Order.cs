using System.Collections;
using UnityEngine;

public class Order : MonoBehaviour
{
    public Enumeration.Order order;
    [SerializeField] SpriteRenderer foodSpriteRenderer;

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        foodSpriteRenderer.sprite = orderManager.OrderSprites[order];
    }
}