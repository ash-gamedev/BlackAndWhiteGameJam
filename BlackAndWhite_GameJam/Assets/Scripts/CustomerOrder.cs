using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    public EnumOrder order;
    [SerializeField] Image spriteImage;    

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
        OrderManager orderManager = FindObjectOfType<OrderManager>();

        AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderPlaced);
        orderManager.AddOrderToQueue(order, replacementOrder: true);
        ScoreKeeper.OrderReplaced();
    }
}