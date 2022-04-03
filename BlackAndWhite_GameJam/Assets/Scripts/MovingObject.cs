using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private TileManager tileConveyerManager;
    private float speed = 0;
    private Vector3 direction;
    public bool canChangeDirection = false; 

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tileConveyerManager = FindObjectOfType<TileManager>();
        speed = tileConveyerManager.ConveyerSpeed;
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

        // if plate falls of the conveyer (and not eaten)
        if (!tileConveyerManager.IsOnConveyerTile(transform.position) && gameObject.transform.parent == null)
        {
            Destroy(gameObject);
        }
    }

    public bool IsMoving()
    {
        if (rigidbody.velocity == Vector2.zero)
            return false;
        return true;
    }


}
