using System.Collections;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private Transform CustomerPosition;
    private bool hasCustomer = false;

    public Transform GetCustomerPosition()
    {
        return CustomerPosition;
    }

    public bool GetHasCustomer()
    {
        return hasCustomer;
    }

    public void SetHasCustomer(bool hasCustomer)
    {
        this.hasCustomer = hasCustomer;
    }
}