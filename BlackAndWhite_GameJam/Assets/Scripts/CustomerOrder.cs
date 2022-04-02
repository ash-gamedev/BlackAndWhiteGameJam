using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public Enumeration.Order order;
    [SerializeField] SpriteRenderer bubbleSpriteRenderer;

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        bubbleSpriteRenderer.sprite = orderManager.OrderSprites[order];
    }

    public Enumeration.Order GetOrder()
    {
        return order;
    }
}