using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TadpoleMove : Action
{
    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tadpoleData = GetComponent<TinyTadpole>();
    }

    public override TaskStatus OnUpdate()
    {
        transform.parent = tadpoleData.ParentRigidbody.transform.parent;
        rigidbody.velocity = tadpoleData.ShootMovingSpeed * tadpoleData.Direction;
        return TaskStatus.Success;
    }

}
