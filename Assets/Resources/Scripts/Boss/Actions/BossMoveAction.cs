using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BossMoveAction : Action
{
    public SharedTransform waypoint;
    public SharedFloat movingSpeed = 5f;
    public SharedFloat movingAccel = 10f;
    private Rigidbody2D rigidbody;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public override TaskStatus OnUpdate()
    {
        Vector2 targetPosition = waypoint.Value.position;
        float distance = Vector2.Distance(targetPosition, rigidbody.position);
        if (distance > 0.1f)
        {
            return TaskStatus.Running;
        }
        else
        {
            Debug.Log("Success");
            transform.position = waypoint.Value.position;
            rigidbody.velocity = Vector2.zero;
            return TaskStatus.Success;
        }
    }

    public override void OnFixedUpdate()
    {
        Vector2 targetPosition = waypoint.Value.position;
        float distance = Vector2.Distance(targetPosition, rigidbody.position);
        Vector2 targetDirection = (targetPosition - rigidbody.position).normalized * movingSpeed.Value * Mathf.Min(1, distance / 0.5f + 0.1f);
        float maxSpeed = movingAccel.Value * Time.deltaTime;
        Vector2 velocity;
        velocity.x = Mathf.MoveTowards(rigidbody.velocity.x, targetDirection.x, maxSpeed);
        velocity.y = Mathf.MoveTowards(rigidbody.velocity.y, targetDirection.y, maxSpeed);
        rigidbody.velocity = velocity;
    }
}
