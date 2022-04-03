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

    GameObject orderBubbleInstance;
    bool setOrder = false;
    bool wasOrderCorrect = false;
    Vector3? targetSeat;

    private void Start()
    {
        Array orders = Enum.GetValues(typeof(EnumOrder));
        System.Random random = new System.Random();

        customerOrder = (EnumOrder)orders.GetValue(random.Next(orders.Length));
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
                Debug.Log("Correct Order!");
            }
            else
            {
                Debug.Log("Incorrect Order!");
                Debug.Log("Customer Order: " + customerOrder.ToString() + " Actual Order: " + order.ToString());
            }

            //Remove order
            Destroy(orderBubbleInstance);

            //Set plate order as child
            collision.transform.parent = this.transform;

            StartCoroutine(FinishEating());
        }
    }

    public void SetOrder()
    {
        // instantiate order bubble
        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        Vector3 orderBubbleSpawn = transform.position + new Vector3(0, 1f, 0);
        orderBubbleInstance = 
            Instantiate(orderBubble,  // what object to instantiate
                        orderBubbleSpawn, // where to spawn the object
                        Quaternion.identity); // need to specify rotation

        // spawn order
        FindObjectOfType<OrderManager>().SpawnOrder(customerOrder);
    }

    public void SetTarget(Vector3 target)
    {
        targetSeat = target;
    }

    public IEnumerator FinishEating()
    {
        yield return new WaitForSeconds(timeToEatInSeconds);
        Leave();
    }

    public void Leave()
    {
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