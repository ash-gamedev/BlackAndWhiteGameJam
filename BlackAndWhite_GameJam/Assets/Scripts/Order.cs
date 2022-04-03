using System.Collections;
using UnityEngine;

public class Order : MonoBehaviour
{
    public EnumOrder order;
    [SerializeField] SpriteRenderer foodSpriteRenderer;

    private Animator animator;
    private MovingObject movingObject;

    private float plateFallingAnimationLength = 2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movingObject = GetComponent<MovingObject>();
    }

    private void Start()
    {
        // wait for animation for finish before beginning moving
        GetPlateAnimationLength();
        Invoke("OnPlateAnimationComplete", plateFallingAnimationLength);

        OrderManager orderManager = FindObjectOfType<OrderManager>();
        foodSpriteRenderer.sprite = orderManager.OrderSprites[order];
    }

    private void GetPlateAnimationLength()
    {
        plateFallingAnimationLength = (animator.runtimeAnimatorController.animationClips[0].length) * 0.8f;
    }

    private void OnPlateAnimationComplete()
    {
        movingObject.canMove = true;
    }
}