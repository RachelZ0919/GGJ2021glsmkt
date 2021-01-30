using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossWarningAnimation : Action
{
    public SharedGameObject animationObject;


    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}
