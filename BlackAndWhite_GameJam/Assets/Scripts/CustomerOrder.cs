using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    public EnumOrder order;
    [SerializeField] Image spriteImage;

    Canvas uiCanvas;

    private void Start()
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        spriteImage.sprite = orderManager.OrderSprites[order];
        spriteImage.SetNativeSize();
    }

    public EnumOrder GetOrder()
    {
        return order;
    }

    public void SpawnOrder()
    {
        AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderPlaced);
        FindObjectOfType<OrderManager>().AddOrderToQueue(order);
    }
}