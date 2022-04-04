using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public EnumOrder order;
    //[SerializeField] SpriteRenderer bubbleSpriteRenderer;

    Canvas uiCanvas;

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        //bubbleSpriteRenderer.sprite = orderManager.OrderSprites[order];
        uiCanvas = FindObjectOfType<Canvas>();
    }

    public EnumOrder GetOrder()
    {
        return order;
    }
}