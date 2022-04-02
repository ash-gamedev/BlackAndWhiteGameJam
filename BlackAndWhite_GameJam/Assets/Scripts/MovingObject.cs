using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private TileConveyerManager tileConveyerManager;
    private float speed = 0;
    private Vector2 direction;

    private void Awake()
    {
        tileConveyerManager = FindObjectOfType<TileConveyerManager>();
        speed = tileConveyerManager.ConveyerSpeed;
    }

    private void Start()
    {
        direction = tileConveyerManager.GetTileDirection(transform.position);
    }

    private void Update()
    {
        Vector3 position = transform.position;

        if (direction == Vector2.right)
            position = new Vector3(position.x - 0.5f, position.y, position.z);
        else if (direction == Vector2.left)
            position = new Vector3(position.x + 0.5f, position.y, position.z);
        else if (direction == Vector2.up)
            position = new Vector3(position.x, position.y - 0.5f, position.z);
        else if (direction == Vector2.down)
            position = new Vector3(position.x, position.y + 0.5f, position.z);

        direction = tileConveyerManager.GetTileDirection(position);

        Vector2 moveVector = direction * Time.deltaTime * speed;
        transform.position += new Vector3(moveVector.x, moveVector.y, 0);
    }
}
