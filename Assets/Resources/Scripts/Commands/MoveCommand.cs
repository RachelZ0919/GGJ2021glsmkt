using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCommand : Command
{
    private Rigidbody2D rigidbody;
    private IMoveInput move;

    public float maxSpeed = 10;
    public float startAcceleration = 300;
    public float stopAcceletation = 1400000;
    private bool isTurning;

    private void Awake()
    {
        rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        move = GetComponent<IMoveInput>();
    }

    public override void Execute()
    {

    }

    private void FixedUpdate()
    {
        float maxSpeedChange;
        if (move.moveDirection == Vector2.zero || isTurning || Vector2.Dot(move.moveDirection.normalized, rigidbody.velocity.normalized) < Mathf.Cos(80))
        {
            maxSpeedChange = stopAcceletation * Time.deltaTime;
            isTurning = true;
        }
        else
        {
            maxSpeedChange = startAcceleration * Time.deltaTime;
        }
        Vector2 desiredVelocity = maxSpeed * move.moveDirection.normalized;
        Vector2 velocity = rigidbody.velocity;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        if (velocity.magnitude >= maxSpeed - 0.5f)
        {
            isTurning = false;
        }
        rigidbody.velocity = velocity;
    }
}
