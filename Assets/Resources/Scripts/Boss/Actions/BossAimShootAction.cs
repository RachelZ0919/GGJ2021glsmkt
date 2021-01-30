using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossAimShootAction : Action
{
    public SharedGameObject shootingObject;
    public SharedFloat attackInter;
    public SharedFloat totalLastTime;
    public SharedTransform aimingTransform;

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
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - actionStartTime < totalLastTime.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            if (Time.time - lastShootingTime >= attackInter.Value)
            {
                shootingBehavior.Shoot(GetDirection());
                lastShootingTime = Time.time;
            }
        }
        return TaskStatus.Running;
    }

    private Vector2 GetDirection()
    {
        Vector2 targetPos = aimingTransform.Value.position;
        Vector2 myPos = shootingBehavior.holdingPoint.position;
        return targetPos - myPos;
    }
}
