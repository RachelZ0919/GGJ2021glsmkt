using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossRotateShootAction : Action
{
    public SharedGameObject shootingObject;
    public SharedFloat attackInter;
    public SharedFloat totalLastTime;
    public SharedFloat turningAngle = 0;
    public SharedFloat startAngle = 0;

    private ShootingBehavior shootingBehavior;
    private float actionStartTime;
    private float lastShootingTime;
    private float angle = 0;

    public override void OnAwake()
    {
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
    }

    public override void OnStart()
    {
        actionStartTime = Time.time;
        lastShootingTime = Time.time - attackInter.Value;
        angle = startAngle.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - actionStartTime < totalLastTime.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            if(Time.time - lastShootingTime >= attackInter.Value)
            {
                shootingBehavior.Shoot(GetDirection());
                angle += turningAngle.Value;
                
                if(angle >= 180)
                {
                    angle -= 360;
                }
                else if(angle <= -180)
                {
                    angle += 360;
                }

                lastShootingTime = Time.time;
            }
        }
        return TaskStatus.Running;
    }

    private Vector2 GetDirection()
    {
        float radius = angle * Mathf.Deg2Rad;
        Vector2 vec = Vector2.right;
        float x = (vec.x * Mathf.Cos(radius) - vec.y * Mathf.Sin(radius));
        float y = (vec.x * Mathf.Sin(radius) + vec.y * Mathf.Cos(radius));
        return new Vector2(x, y);
    }
}
