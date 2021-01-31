using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;


public class BossAimShootAction : Action
{
    public SharedGameObject shootingObject;
    public SharedFloat attackInter;
    public SharedFloat totalLastTime;
    public SharedTransform aimingTransform;

    public SharedFloat addtionalRange = 0;
    public SharedFloat addtionalSpeed = 0;
    public SharedFloat additionalLasttime = 0;

    public SharedBool needAttackAnimation;

    private ShootingBehavior shootingBehavior;
    private UnityArmatureComponent armatureComponent;
    private float actionStartTime;
    private float lastShootingTime;
    private float angle = 0;

    public override void OnAwake()
    {
        armatureComponent = transform.Find("anim").GetComponent<UnityArmatureComponent>();
    }

    public override void OnStart()
    {
        if (needAttackAnimation.Value)
        {
            armatureComponent.AddDBEventListener(EventObject.LOOP_COMPLETE, OnAttackAnimationEnd);
            armatureComponent.animation.Play("bubble_attack");
        }
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
        actionStartTime = Time.time;
        lastShootingTime = Time.time - attackInter.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - actionStartTime >= totalLastTime.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            if (Time.time - lastShootingTime >= attackInter.Value)
            {
                UpdateAdditionalSettings();
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

    private void UpdateAdditionalSettings()
    {
        shootingBehavior.setting.range = addtionalRange.Value;
        shootingBehavior.setting.speed = addtionalSpeed.Value;
        shootingBehavior.setting.lastTime = additionalLasttime.Value;
    }

    private void OnAttackAnimationEnd(string type, EventObject eventObject)
    {
        if(eventObject.animationState.name == "bubble_attack")
        {
            armatureComponent.animation.Play("idle");
            armatureComponent.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnAttackAnimationEnd);
        }
    }
}
