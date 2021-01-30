using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;

public class TadpoleGoBack : Action
{
    public SharedGameObject animationObject;

    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;
    private UnityArmatureComponent unityArmature;
    private ShootingBehavior shootingBehavior;

    public SharedFloat predictStep = Time.deltaTime * 2f;
    public SharedFloat accel = 40f;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tadpoleData = GetComponent<TinyTadpole>();
        unityArmature = animationObject.Value.GetComponent<UnityArmatureComponent>();
        shootingBehavior = GetComponent<ShootingBehavior>();
    }

    public override void OnStart()
    {
        unityArmature.animation.Play("newAnimation");
        tadpoleData.IsBacking = true;
        tadpoleData.HasHit = false;
    }

    public override TaskStatus OnUpdate()
    {
        float distance = Vector2.Distance(tadpoleData.ParentRigidbody.position, rigidbody.position);
        if (distance < 1f)
        {
            tadpoleData.TadpoleGroup.Comeback(tadpoleData);
            tadpoleData.IsBacking = false;
            tadpoleData.HasHit = true;
            return TaskStatus.Success;
        }

        float speed = tadpoleData.CallbackSpeed * Mathf.Min(1, (distance - 0.3f) / 1.5f + 0.4f);
        Vector2 targetVel = (tadpoleData.ParentRigidbody.position + tadpoleData.ParentRigidbody.velocity * predictStep.Value - rigidbody.position).normalized * speed;
        float maxSpeedChange = accel.Value * Time.deltaTime;
        
        float vel_x = rigidbody.velocity.x;
        float vel_y = rigidbody.velocity.y;
        Vector2 velocity;
        velocity.x = Mathf.MoveTowards(vel_x, targetVel.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(vel_y, targetVel.y, maxSpeedChange);

        rigidbody.velocity = velocity;

        shootingBehavior.UpdateDirection(velocity.normalized);

        return TaskStatus.Running;
    }
}
