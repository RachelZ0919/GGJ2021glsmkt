using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;

public class TadpoleForceSleep : Action
{
    public SharedGameObject animationObject;

    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;
    private UnityArmatureComponent unityArmature;

    public SharedFloat accel = 60f;
    public SharedFloat scatterSpeed = 20f;

    private bool animationEnd;
    private bool startAnimation;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tadpoleData = GetComponent<TinyTadpole>();
        unityArmature = animationObject.Value.GetComponent<UnityArmatureComponent>();
    }

    public override void OnStart()
    {
        tadpoleData.IsShooting = false;
        tadpoleData.HasHit = true;
        tadpoleData.IsBacking = false;
        animationEnd = false;
        startAnimation = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (!startAnimation)
        {
            float distance = Vector2.Distance(tadpoleData.FollowTransform.position, rigidbody.position);
            if (distance < 0.2f)
            {
                rigidbody.velocity = Vector2.zero;
                startAnimation = true;
                unityArmature.animation.Play("die");
                unityArmature.animation.timeScale = 2;
                unityArmature.AddDBEventListener(EventObject.LOOP_COMPLETE, OnDieAnimationEnd);

            }
            else
            {
                Debug.Log($"Not near enough! distance:{distance}");
            }
        }
        else
        {
            if (animationEnd)
            {
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }

    public override void OnFixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (tadpoleData.FollowTransform != null)
        {
            Debug.Log("Following……");
            float distance = Vector2.Distance(tadpoleData.FollowTransform.position, rigidbody.position);
            if (distance < 0.1f)
            {
                Debug.Log("IsNearEnough!");
                rigidbody.velocity = Vector2.zero;
                return;
            }

            float speed = scatterSpeed.Value * Mathf.Min(1, distance / 1.5f);
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
            Debug.LogError("Follow Transform is null!");
        }
    }

    private void OnDieAnimationEnd(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name == "die")
        {
            unityArmature.animation.timeScale = 1;
            unityArmature.animation.Play("die-loading");
            unityArmature.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnDieAnimationEnd);
            animationEnd = true;
        }
    }
}
