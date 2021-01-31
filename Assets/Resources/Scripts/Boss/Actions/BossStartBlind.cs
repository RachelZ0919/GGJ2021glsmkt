using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossStartBlind : Action
{
    public SharedFloat lastTime;

    private float startTime;

    public override void OnStart()
    {
        startTime = Time.time;
        BlindPointLight.instance.StartBlind();
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= lastTime.Value)
        {
            BlindPointLight.instance.EndBlind();
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
