using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{    
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] public float timeToEatInSeconds = 2f;
    [SerializeField] GameObject tipObject;
    [SerializeField] GameObject unHappyObject;
    
    public float timeBeforeLeaving = 0;

    EnumOrder customerOrder;
    CustomerWaitingTimer customerWaitingTimer = null;

    Seat seatInstance = null;
    GameObject orderBubbleInstance = null;
    bool setOrder = false;
    bool wasOrderCorrect = false;
    bool orderReceived = false;
    bool isLeaving = false;
    float timeOrderReceived = 0f;
    Vector3 startPosition;
    Vector3? walkingTarget = null;

    GameObject orderInstance = null;
    GameObject orderBubble = null;

    UIManager uiManager;

    private void Start()
    {
        try
        {
            // set start position
            startPosition = transform.position;

            // create random order
            Array orders = Enum.GetValues(typeof(EnumOrder));
            System.Random random = new System.Random();
            customerOrder = (EnumOrder)orders.GetValue(random.Next(orders.Length));

            seatInstance = transform.parent.gameObject.GetComponent<Seat>();

            // time before leaving
            timeBeforeLeaving = LevelManager.TimeBeforeCustomerLeaves;

            // get ui manager
            uiManager = FindObjectOfType<UIManager>();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void Update()
    {
        try
        {
            // move towards seat
            if (transform.position != walkingTarget)
            {
                MoveTowards(walkingTarget);
            }

            // if seated and order hasn't been placed
            if (transform.position == walkingTarget && !setOrder)
            {
                setOrder = true;
                SetOrder();
            }

            // if leaving 
            else if (transform.position == walkingTarget && isLeaving)
            {
                // remove
                Destroy(gameObject);
            }

            // if customer waiting for too long, leave
            if (customerWaitingTimer != null && !isLeaving)
            {
                if (customerWaitingTimer.timerSlider.value <= 0)
                {
                    Leave();
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EnumTags.Order.ToString()) && !orderReceived)
        {
            orderReceived = true;
            timeOrderReceived = customerWaitingTimer.currentTime;

            orderInstance = collision.gameObject;

            // replace layer to not collide with others
            orderInstance.layer = gameObject.layer;

            EnumOrder order = collision.GetComponent<Order>().order;
            
            if (order == customerOrder)
            {
                //AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderCorrect);
                wasOrderCorrect = true;
                StartCoroutine(CompleteOrder(collision.gameObject));
            }
            else
            {
                Leave();
            }
        }
    }

    public void SetOrder()
    {
        //sound effect
        AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerOrder);

        // instantiate object on UI
        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        orderBubbleInstance = uiManager.InstantiateObjectOnUi(seatInstance.GetCustomerOrderBubbleSpawnPosition(), orderBubble);

        // set conveyer to face customer after order is placed
        seatInstance.SetConveyerTileToFaceCustomer();

        // get time
        customerWaitingTimer = orderBubbleInstance.GetComponent<CustomerWaitingTimer>();

        // spawn order
        FindObjectOfType<OrderManager>().AddOrderToQueue(customerOrder);
    }

    public IEnumerator CompleteOrder(GameObject order)
    {
        //Remove order
        Destroy(orderBubbleInstance);

        // correct order
        if (wasOrderCorrect)
        {
            ScoreKeeper.OrderCompleted();

            // tip
            int tip = ScoreKeeper.AddTip(timeOrderReceived);
            InstantiateTip(tip);
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.CustomerPays);

            //Set plate order as child
            order.transform.parent = this.transform;

            MovingObject movingPlate = order.GetComponent<MovingObject>();
            movingPlate.canChangeDirection = false;

            seatInstance.ResetTileConveyerToOriginalPath();
        }

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
        // customer waiting too long [order not received] OR wrong order
        if (!wasOrderCorrect)
        {
            seatInstance.ResetTileConveyerToOriginalPath();
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.OrderIncorrect);
            InstantiateUnhappyFace();
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

    private void InstantiateTip(int tip)
    {
        GameObject tipObjectInstance = uiManager.InstantiateObjectOnUi(seatInstance.GetCustomerOrderBubbleSpawnPosition(), tipObject);

        // Update tip text
        string tipText = $"+{tip} Tip";

        tipObjectInstance.GetComponentInChildren<TextMeshProUGUI>().text = tipText;
    }

    private void InstantiateUnhappyFace()
    {
        uiManager.InstantiateObjectOnUi(seatInstance.GetCustomerOrderBubbleSpawnPosition(), unHappyObject);
    }
}