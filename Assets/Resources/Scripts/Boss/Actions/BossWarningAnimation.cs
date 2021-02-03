using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossWarningAnimation : Action
{
    public SharedGameObject animationObject;
    public SharedFloat warningTime;

    private float startTime;


    public override void OnStart()
    {
        if(animationObject.Value != null)
        {
            animationObject.Value.SetActive(true);
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
