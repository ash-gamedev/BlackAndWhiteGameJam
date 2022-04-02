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

        // only want to change directions when object is centered (x position and y position divisible by 0.5)
        if (Math.Round(position.x, 4) % 0.5 == 0 && Math.Round(position.y, 4) % 0.5 == 0)
        {
            Debug.Log(Math.Round(position.x, 4) + " " + Math.Round(position.y, 4));
            direction = tileConveyerManager.GetTileDirection(transform.position);
        }
            

        transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
    }
}
