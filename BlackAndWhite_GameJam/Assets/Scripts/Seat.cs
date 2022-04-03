using System.Collections;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private Transform CustomerPosition;
    [SerializeField] EnumSeatPosition seatPosition = EnumSeatPosition.Bottom;

    private TileManager tileManager;
    private Vector3Int seatGridPosition;
    public Vector3Int tileConveyerPosition;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        seatGridPosition = tileManager.GetTileGridPosition(transform.position);
        tileConveyerPosition = GetClosestConveyerTilePosition();
    }

    public Transform GetCustomerPosition()
    {
        return CustomerPosition;
    }

    public bool GetCustomer()
    {
        Customer customer = GetComponentInChildren<Customer>();

        // if customer doesnt exist as child return false
        if (customer == null) return false;
        // otherwise customer exists
        return true;
    }

    public void SetCustomer(GameObject customer)
    {
        // if no customer is sitting in chair, instantiate customer
        if(GetCustomer() == false)
        {
            Vector3 spawnPosition = GetCustomerSpawnPosition();
            GameObject customerInstance =
                    Instantiate(customer,  // what object to instantiate
                        spawnPosition, // where to spawn the object
                        Quaternion.identity, // need to specify rotation
                        transform); // create child under

            // rotate to face seat + set seat target
            customerInstance.transform.right = transform.position - new Vector3(customerInstance.transform.position.x, customerInstance.transform.position.y, 0);
            customerInstance.GetComponent<Customer>().SetTarget(transform.position);
        }
    }

    private Vector3Int GetClosestConveyerTilePosition()
    {
        if (seatPosition == EnumSeatPosition.Top) return seatGridPosition + (Vector3Int.down * 2);
        else if (seatPosition == EnumSeatPosition.Bottom) return seatGridPosition + (Vector3Int.up * 2);
        else if (seatPosition == EnumSeatPosition.Right) return seatGridPosition + (Vector3Int.left * 2);
        else return seatGridPosition + (Vector3Int.right * 2);
    }

    private Vector3 GetCustomerSpawnPosition()
    {
        if (seatPosition == EnumSeatPosition.Top) return transform.position + (Vector3Int.up * 4);
        else if (seatPosition == EnumSeatPosition.Bottom) return transform.position + (Vector3Int.down * 4);
        else if (seatPosition == EnumSeatPosition.Right) return transform.position + (Vector3Int.right * 4);
        else return transform.position + (Vector3Int.left * 4);
    }
}