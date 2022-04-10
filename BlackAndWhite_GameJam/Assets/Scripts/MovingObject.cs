using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] float timeBeforeDissappearOnBreak = 1f;
    private Rigidbody2D rigidbody;
    private TileManager tileConveyerManager;
    private float speed = 0;
    private Vector3 direction;
    public bool canChangeDirection = true;
    private bool plateFell = false;

    private Animator animator;
    private EnumAnimationState animationState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        tileConveyerManager = FindObjectOfType<TileManager>();
        speed = LevelManager.ConveyorSpeed;
    }

    private void Start()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        Vector3 position = transform.position;

        // Use position 0.5 away from current position (to keep tile in center of conveyor on loop)
        if (direction == Vector3.right)
            position = new Vector3(position.x - 0.5f, position.y, position.z);
        else if (direction == Vector3.left)
            position = new Vector3(position.x + 0.5f, position.y, position.z);
        else if (direction == Vector3.up)
            position = new Vector3(position.x, position.y - 0.5f, position.z);
        else if (direction == Vector3.down)
            position = new Vector3(position.x, position.y + 0.5f, position.z);

        Vector3 newDirection = tileConveyerManager.GetTileDirection(position);

        // if can move, set direction as new direction
        if (canChangeDirection || newDirection == Vector3.zero)
            direction = newDirection;
        
        Vector3 moveVelocity = direction * speed;
        rigidbody.velocity = moveVelocity;

        // if plate falls off the conveyer (and not eaten)
        if ((!tileConveyerManager.IsOnConveyerTile(transform.position) || tileConveyerManager.IsOnGarbageTile(transform.position)) && gameObject.transform.parent == null && plateFell == false)
        {
            plateFell = true;
            canChangeDirection = false;
            StartCoroutine(PlateFalls());            
        }
    }

    public bool IsMoving()
    {
        if (rigidbody.velocity == Vector2.zero)
            return false;
        return true;
    }

    private IEnumerator PlateFalls()
    {
        // todo change sort order for sprite: InteractableConveyerBelt
        yield return new WaitUntil(() => direction == Vector3.zero);

        // if not garbage tile (break plate)
        if (!tileConveyerManager.IsOnGarbageTile(transform.position))
        {
            // sound effect
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.PlateShatter);

            // animation
            ChangeAnimationState(EnumAnimationState.PlateBreak);

            // score
            ScoreKeeper.PlateBroken();
        }
        // is garbage tile
        else
        {
            // sound effect
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.Trash);

            // animation
            ChangeAnimationState(EnumAnimationState.PlateTrash);
        }

        // wait & destroy
        yield return new WaitForSeconds(timeBeforeDissappearOnBreak);
        Destroy(gameObject);
    }

    void ChangeAnimationState(EnumAnimationState newState)
    {
        //stop the same animation from interuptting itself
        if (animationState == newState) return;

        //play the animation
        animator.Play(newState.ToString());

        //reassign current state
        animationState = newState;
    }
}
