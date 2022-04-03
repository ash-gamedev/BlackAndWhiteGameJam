using System.Collections;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] EnumSeatPosition seatPosition = EnumSeatPosition.Bottom;

    private TileManager tileManager;
    private Vector3Int seatGridPosition;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        seatGridPosition = tileManager.GetTileGridPosition(transform.position);
    }

    #region public functions
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
        if (GetCustomer() == false)
        {
            Vector3 spawnPosition = GetCustomerSpawnPosition();
            GameObject customerInstance =
                    Instantiate(customer,  // what object to instantiate
                        spawnPosition, // where to spawn the object
                        Quaternion.identity,
                        transform); //, // need to specify rotation

            // rotate to face seat + set seat target
            customerInstance.transform.right = transform.position - new Vector3(customerInstance.transform.position.x, customerInstance.transform.position.y, 0);
            customerInstance.GetComponent<Customer>().SetTarget(transform.position);
        }
    }

    public void SetConveyerTileToFaceCustomer()
    {
        EnumTileDirection conveyerTileDirection = GetConveyerTileDirection();
        Vector3Int conveyerTilePosition = GetClosestConveyerTilePosition();

        tileManager.SetTileOnConveyer(conveyerTilePosition, conveyerTileDirection);
    }

    public void ResetTileConveyerToOriginalPath()
    {
        Vector3Int conveyerTilePosition = GetClosestConveyerTilePosition();

        tileManager.ResetTileOnConveyer(conveyerTilePosition);
    }
    #endregion

    #region private functions
    
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

    private EnumTileDirection GetConveyerTileDirection()
    {
        if (seatPosition == EnumSeatPosition.Top) return EnumTileDirection.Up;
        else if (seatPosition == EnumSeatPosition.Bottom) return EnumTileDirection.Down;
        else if (seatPosition == EnumSeatPosition.Right) return EnumTileDirection.Right;
        else return EnumTileDirection.Left;
    }
    #endregion

}