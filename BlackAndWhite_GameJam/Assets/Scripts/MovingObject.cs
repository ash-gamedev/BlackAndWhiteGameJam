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
    public bool canMove = false; 

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tileConveyerManager = FindObjectOfType<TileManager>();
        speed = tileConveyerManager.ConveyerSpeed;
    }

    private void Start()
    {
        direction = tileConveyerManager.GetTileDirection(transform.position);
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3 position = transform.position;

            if (direction == Vector3.right)
                position = new Vector3(position.x - 0.5f, position.y, position.z);
            else if (direction == Vector3.left)
                position = new Vector3(position.x + 0.5f, position.y, position.z);
            else if (direction == Vector3.up)
                position = new Vector3(position.x, position.y - 0.5f, position.z);
            else if (direction == Vector3.down)
                position = new Vector3(position.x, position.y + 0.5f, position.z);

            direction = tileConveyerManager.GetTileDirection(position);

            Vector3 moveVelocity = direction * speed;
            rigidbody.velocity = moveVelocity;
        }
    }


}
