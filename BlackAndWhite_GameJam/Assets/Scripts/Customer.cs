using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //private CustomerOrder customerOrder;
    [SerializeField] Enumeration.Order customerOrder;

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
                Debug.Log("Incorrect Order!");
        }
    }
}