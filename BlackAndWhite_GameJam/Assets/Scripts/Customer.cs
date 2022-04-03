using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] public float timeToEatInSeconds = 2f;
    [SerializeField] public float timeBeforeLeaving = 100f;

    [SerializeField] EnumOrder customerOrder;
    [SerializeField] GameObject orderBubble;

    GameObject orderBubbleInstance;
    bool setOrder = false;
    bool wasOrderCorrect = false;

    private void Start()
    {
        customerOrder = EnumOrder.Hotdog;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
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
        else if (collision.CompareTag(EnumTags.Chair.ToString()) && setOrder == false)
        {
            setOrder = true;
            SetOrder(collision.gameObject.transform);
        }
    }

    public void SetOrder(Transform chairTransform)
    {
        // instantiate order bubble
        orderBubble.GetComponent<CustomerOrder>().order = customerOrder; // set order
        Vector3 orderBubbleSpawn = chairTransform.position + new Vector3(0, 1.25f, 0);
        orderBubbleInstance = 
            Instantiate(orderBubble,  // what object to instantiate
                        orderBubbleSpawn, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
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
}