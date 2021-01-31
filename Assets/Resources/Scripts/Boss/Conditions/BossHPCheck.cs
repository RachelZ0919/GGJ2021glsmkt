using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossHPCheck : Conditional
{
    public SharedFloat targetHP;

    private Stats stat;
    public override void OnAwake()
    {
        stat = GetComponent<Stats>();
    }

    public override TaskStatus OnUpdate()
    {
        if(targetHP.Value < stat.health)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
