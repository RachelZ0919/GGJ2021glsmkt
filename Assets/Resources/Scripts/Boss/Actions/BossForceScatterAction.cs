using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossForceScatterAction : Action
{
    public SharedTransform scatterPointsParentTransform;
    public SharedGameObject tadpoleGroupObject;

    private TadpoleGroup tadpoles;

    public override void OnAwake()
    {
        tadpoles = tadpoleGroupObject.Value.GetComponent<TadpoleGroup>();
    }

    public override TaskStatus OnUpdate()
    {
        tadpoles.ForceScatter(scatterPointsParentTransform.Value);
        return TaskStatus.Success;
    }
}
