using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private TileManager tileConveyerManager;
    private float speed = 0;
    private Vector3 direction;

    private void Awake()
    {
        tileConveyerManager = FindObjectOfType<TileManager>();
        speed = tileConveyerManager.ConveyerSpeed;
    }

    private void Start()
    {
        direction = tileConveyerManager.GetTileDirection(transform.position);
    }

    private void Update()
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

        Vector3 moveVector = direction * Time.deltaTime * speed;
        transform.position += moveVector;
    }
}
