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
    bool orderReceived = false;
    bool isLeaving = false;
    Vector3 startPosition;
    Vector3? walkingTarget;

    GameObject orderInstance;

    private void Start()
    {
        // set start position
        startPosition = transform.position;

        // create random order
        Array orders = Enum.GetValues(typeof(EnumOrder));
        System.Random random = new System.Random();

        // spawn order
        customerOrder = (EnumOrder)orders.GetValue(random.Next(orders.Length));

        seatInstance = transform.parent.gameObject.GetComponent<Seat>();
    }

    private void Update()
    {
        // move towards seat
        if(transform.position != walkingTarget)
        {
            MoveTowards(walkingTarget);
        }

        // if seated and order hasn't been placed
        if(transform.position == walkingTarget && !setOrder)
        {
            setOrder = true;
            SetOrder();
        }

        // if leaving (order already set)
        else if(transform.position == walkingTarget && isLeaving)
        {
            // remove
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EnumTags.Order.ToString()) && !orderReceived)
        {
            orderReceived = true;
            orderInstance = collision.gameObject;
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
                        Quaternion.identity,
                        transform); // need to specify rotation

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
        walkingTarget = target;
    }

    public void Leave()
    {
        if (wasOrderCorrect)
        {
            ScoreKeeper.AddToScore(10);
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerPays);
        }

        // destroy plate
        Destroy(orderInstance);

        // walk away from seat
        transform.right = startPosition - new Vector3(transform.position.x, transform.position.y, 0);
        walkingTarget = startPosition;
        isLeaving = true;
    }

    private void MoveTowards(Vector3? target)
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)target, moveSpeed * Time.deltaTime);
        }
    }
}