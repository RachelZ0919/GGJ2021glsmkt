using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;

public class BossRageAction : Action
{
    private UnityArmatureComponent armatureComponent;
    private bool animEnd;


    public override void OnAwake()
    {
        armatureComponent = transform.Find("anim").GetComponent<UnityArmatureComponent>();
    }

    public override void OnStart()
    {
        armatureComponent.AddDBEventListener(EventObject.LOOP_COMPLETE, OnAnimationEnd);
        armatureComponent.animation.Play("rage");
        animEnd = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (animEnd)
        {
            GameManager.instance.LevelFailed();
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }

    private void OnAnimationEnd(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name == "rage") 
        {
            animEnd = true;
            armatureComponent.animation.GotoAndStopByProgress("rage", 1);
            armatureComponent.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnAnimationEnd);
        }
    }
}
