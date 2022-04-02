using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public EnumOrder order;
    [SerializeField] SpriteRenderer bubbleSpriteRenderer;

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        bubbleSpriteRenderer.sprite = orderManager.OrderSprites[order];
    }

    public EnumOrder GetOrder()
    {
        return order;
    }
}