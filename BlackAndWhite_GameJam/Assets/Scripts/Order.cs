using System.Collections;
using UnityEngine;

public class Order : MonoBehaviour
{
    public EnumOrder order;
    [SerializeField] SpriteRenderer foodSpriteRenderer;

    
    private MovingObject movingObject;

    private void Awake()
    {
        movingObject = GetComponent<MovingObject>();
    }

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        foodSpriteRenderer.sprite = orderManager.OrderSprites[order];
    }

    
}