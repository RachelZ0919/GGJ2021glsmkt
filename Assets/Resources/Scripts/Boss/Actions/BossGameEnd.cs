using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossGameEnd : Action
{
    public override TaskStatus OnUpdate()
    {
        GameManager.instance.LevelFailed();
        return TaskStatus.Failure;
    }
}
