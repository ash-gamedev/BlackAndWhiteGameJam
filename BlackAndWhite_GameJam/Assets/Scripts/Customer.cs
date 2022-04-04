using System;
using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] GameObject orderBubble;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] public float timeToEatInSeconds = 2f;
    [SerializeField] public float timeBeforeLeaving = 100f;

    EnumOrder customerOrder;

    Seat seatInstance;
    GameObject orderBubbleInstance;
    bool setOrder = false;
    bool wasOrderCorrect = false;
    Vector3? targetSeat;

    private void Start()
    {
        Array orders = Enum.GetValues(typeof(EnumOrder));
        System.Random random = new System.Random();

        customerOrder = (EnumOrder)orders.GetValue(random.Next(orders.Length));

        seatInstance = transform.parent.gameObject.GetComponent<Seat>();
    }

    private void Update()
    {
        // move towards seat
        if(transform.position != targetSeat)
        {
            MoveTowards(targetSeat);
        }

        // if seated and order hasn't been placed
        if(transform.position == targetSeat && !setOrder)
        {
            setOrder = true;
            SetOrder();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EnumTags.Order.ToString()))
        {
            EnumOrder order = collision.GetComponent<Order>().order;

            if (order == customerOrder)
            {
                AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderCorrect);
                wasOrderCorrect = true;
            }
            else
            {
                AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderIncorrect);
            }

            StartCoroutine(CompleteOrder(collision.gameObject));
        }
    }

    public void SetOrder()
    {
        //sound effect
        AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerOrder);

        // instantiate order bubble
        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        Vector3 orderBubbleSpawn = transform.position + new Vector3(0, 1f, 0);
        orderBubbleInstance = 
            Instantiate(orderBubble,  // what object to instantiate
                        orderBubbleSpawn, // where to spawn the object
                        Quaternion.identity); // need to specify rotation

        // set conveyer to face customer after order is placed
        seatInstance.SetConveyerTileToFaceCustomer();

        // spawn order
        FindObjectOfType<OrderManager>().SpawnOrder(customerOrder);
    }

    public IEnumerator CompleteOrder(GameObject order)
    {
        //Remove order
        Destroy(orderBubbleInstance);

        //Set plate order as child
        order.transform.parent = this.transform;

        MovingObject movingPlate = order.GetComponent<MovingObject>();
        movingPlate.canChangeDirection = false;

        seatInstance.ResetTileConveyerToOriginalPath();

        yield return new WaitForSeconds(timeToEatInSeconds);
        Leave();
    }

    public void SetTarget(Vector3 target)
    {
        targetSeat = target;
    }

    public void Leave()
    {
        if (wasOrderCorrect) AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerPays);

        // if orderWasCorrect give tip
        Destroy(gameObject);
    }

    private void MoveTowards(Vector3? target)
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)target, moveSpeed * Time.deltaTime);
        }
    }
}