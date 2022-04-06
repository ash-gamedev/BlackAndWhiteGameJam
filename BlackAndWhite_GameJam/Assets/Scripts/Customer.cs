using System;
using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{    
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] public float timeToEatInSeconds = 2f;
    [SerializeField] public float timeBeforeLeaving = 100f;

    EnumOrder customerOrder;
    CustomerWaitingTimer customerWaitingTimer;

    Seat seatInstance;
    GameObject orderBubbleInstance;
    bool setOrder = false;
    bool wasOrderCorrect = false;
    bool orderReceived = false;
    bool isLeaving = false;
    Vector3 startPosition;
    Vector3? walkingTarget;

    GameObject orderInstance;
    GameObject orderBubble;

    private void Start()
    {
        // set start position
        startPosition = transform.position;

        // create random order
        Array orders = Enum.GetValues(typeof(EnumOrder));
        System.Random random = new System.Random();
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

        // if leaving 
        else if(transform.position == walkingTarget && isLeaving)
        {
            // remove
            Destroy(gameObject);
        }

        // if customer waiting for too long, leave
        if (customerWaitingTimer != null && !isLeaving)
        {
            if(customerWaitingTimer.timerSlider.value <= 0)
            {
                Leave();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EnumTags.Order.ToString()) && !orderReceived)
        {
            orderReceived = true;
            orderInstance = collision.gameObject;

            // replace layer to not collide with others
            orderInstance.layer = gameObject.layer;

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

        //first you need the RectTransform component of your canvas
        Canvas uiCanvas = FindObjectOfType<Canvas>();
        RectTransform CanvasRect = uiCanvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(seatInstance.GetCustomerOrderBubbleSpawnPosition());
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y)));

        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        Vector3 orderBubbleSpawn = transform.position + new Vector3(0, 1f, 0);
        orderBubbleInstance = Instantiate(orderBubble, WorldObject_ScreenPosition, Quaternion.identity, uiCanvas.transform);

        // set conveyer to face customer after order is placed
        seatInstance.SetConveyerTileToFaceCustomer();

        // get time
        customerWaitingTimer = orderBubbleInstance.GetComponent<CustomerWaitingTimer>();

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

    public void SetOrderBubble(GameObject orderBubbleObject)
    {
        orderBubble = orderBubbleObject;
    }

    public void Leave()
    {
        Debug.Log("Leaving");

        // correct order
        if (wasOrderCorrect)
        {
            ScoreKeeper.AddToScore(10);
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerPays);
        }
        // customer waiting too long [order not received]
        else if (!orderReceived)
        {
            seatInstance.ResetTileConveyerToOriginalPath();
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderIncorrect);
        }

        // destroy plate
        if(orderInstance != null)
            Destroy(orderInstance);

        //Remove order
        if(orderBubbleInstance != null)
            Destroy(orderBubbleInstance);

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