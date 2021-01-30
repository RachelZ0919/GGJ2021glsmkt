using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TadpoleAttack : Action
{
    private TinyTadpole tadpoleData;
    private Rigidbody2D rigidbody;
    public SharedFloat accel = 20f;
    private ShootingBehavior shootingBehavior;

    private float shootingStartTime;

    public override void OnAwake()
    {
        tadpoleData = GetComponent<TinyTadpole>();
        rigidbody = GetComponent<Rigidbody2D>();
        shootingBehavior = GetComponent<ShootingBehavior>();
    }

    public override void OnStart()
    {
        shootingStartTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        Follow();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        if (objects.Length != 0)
        {
            float minDistance = Mathf.Infinity;
            Transform targetTransform = objects[0].transform;
            for (int i = 0; i < objects.Length; i++)
            {
                float dist = Vector3.Distance(objects[i].transform.position, transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    targetTransform = objects[i].transform;
                }
            }
            Vector2 direction = targetTransform.position - transform.position;

            if (Time.time - shootingStartTime > tadpoleData.ShootingInter)
            {
                Attack(direction);
            }
            else
            {
                shootingBehavior.UpdateDirection(direction.normalized);
            }
        }


        return TaskStatus.Running;
    }

    private void Follow()
    {
        if(tadpoleData.FollowTransform != null)
        {
            float distance = Vector2.Distance(tadpoleData.FollowTransform.position, rigidbody.position);
            if (distance < 0.2f)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }

            float speed = tadpoleData.FollowSpeed * Mathf.Min(1, (distance - 0.3f) / 1.5f);
            Vector2 followPosition = tadpoleData.FollowTransform.position;
            Vector2 targetVel = (followPosition - rigidbody.position).normalized * speed;
            float maxSpeedChange = accel.Value * Time.deltaTime;

            float vel_x = rigidbody.velocity.x;
            float vel_y = rigidbody.velocity.y;
            Vector2 velocity;
            velocity.x = Mathf.MoveTowards(vel_x, targetVel.x, maxSpeedChange);
            velocity.y = Mathf.MoveTowards(vel_y, targetVel.y, maxSpeedChange);

            rigidbody.velocity = velocity;
        }
        else
        {
            tadpoleData.FollowTransform = tadpoleData.TadpoleGroup.GetAFollowPoint();
            rigidbody.velocity = Vector2.zero;
        }
        
    }

    private void Attack(Vector2 direction)
    {
        shootingBehavior.Shoot(direction.normalized);
        shootingStartTime = Time.time;
    }
}
