using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //private CustomerOrder customerOrder;
    [SerializeField] Enumeration.Order customerOrder;
    [SerializeField] GameObject orderBubble;

    GameObject orderBubbleInstance;

    private void Start()
    {
        SetOrder(Enumeration.Order.Hotdog);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag(Enumeration.Tags.Order.ToString()))
        {
            Enumeration.Order order = collision.GetComponent<Order>().order;

            if (order == customerOrder)
            {
                Debug.Log("Correct Order!");
            }
            else
            {
                Debug.Log("Incorrect Order!");
                Debug.Log("Customer Order: " + customerOrder.ToString() + " Actual Order: " + order.ToString());
            }

            Destroy(orderBubbleInstance);
        }
    }

    public void SetOrder(Enumeration.Order order)
    {
        customerOrder = order;

        // instantiate order bubble
        orderBubble.GetComponent<CustomerOrder>().order = order; // set order
        Vector3 orderBubbleSpawn = transform.position + new Vector3(0, 1f, 0);
        orderBubbleInstance = 
            Instantiate(orderBubble,  // what object to instantiate
                        orderBubbleSpawn, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
    }
}