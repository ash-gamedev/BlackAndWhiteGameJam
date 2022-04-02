using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //private CustomerOrder customerOrder;
    [SerializeField] EnumOrder customerOrder;
    [SerializeField] GameObject orderBubble;

    GameObject orderBubbleInstance;
    bool setOrder = false;

    private void Start()
    {
        customerOrder = EnumOrder.Hotdog;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag(EnumTags.Order.ToString()))
        {
            EnumOrder order = collision.GetComponent<Order>().order;

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
        else if (collision.CompareTag(EnumTags.Chair.ToString()) && setOrder == false)
        {
            setOrder = true;
            SetOrder(collision.gameObject.transform);
        }
    }

    public void SetOrder(Transform chairTransform)
    {
        // instantiate order bubble
        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        Vector3 orderBubbleSpawn = chairTransform.position + new Vector3(0, 1.25f, 0);
        orderBubbleInstance = 
            Instantiate(orderBubble,  // what object to instantiate
                        orderBubbleSpawn, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
    }
}