using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;

public class BossWarningAnimation : Action
{
    public SharedGameObject animationObject;
    public SharedFloat warningTime;

    private UnityArmatureComponent anim;
    private float startTime;

    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        if(animationObject.Value != null)
        {
            animationObject.Value.SetActive(true);

            anim = animationObject.Value.GetComponent<UnityArmatureComponent>();
            if (anim != null)
            {
                anim.animation.GotoAndPlayByTime(anim.animation.lastAnimationName, 0);
            }
        }
        else
        {
            Debug.LogError("Animation warning hasn't been set yet!");
        }

        startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= warningTime.Value)
        {
            if (animationObject.Value != null) animationObject.Value.SetActive(false);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
