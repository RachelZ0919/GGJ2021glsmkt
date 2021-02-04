using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;


public class TadpoleSleep : Action
{
    public SharedGameObject animationObject;
    public SharedString animationName;
    private UnityArmatureComponent unityArmature;
    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;

    public override void OnAwake()
    {
        tadpoleData = GetComponent<TinyTadpole>();
        rigidbody = GetComponent<Rigidbody2D>();
        unityArmature = animationObject.Value.GetComponent<UnityArmatureComponent>();
    }

    public override TaskStatus OnUpdate()
    {
        tadpoleData.IsShooting = false;
        tadpoleData.HasHit = true;
        tadpoleData.IsSleeping = true;
        rigidbody.velocity = Vector2.zero;
        unityArmature.animation.Play(animationName.Value);
        return TaskStatus.Success;
    }
}
