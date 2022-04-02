using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    private Enumeration.Order order;

    private void Start()
    {
        // select order
    }

    public Enumeration.Order GetOrder()
    {
        return order;
    }
}