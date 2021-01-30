using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TadpoleDefend : Action
{
    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;
    public SharedFloat accel = 20f;
    public SharedFloat rotateSpeed = 60f;
    private bool isOnPoint;
    private float lastVel;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tadpoleData = GetComponent<TinyTadpole>();
    }

    public override void OnStart()
    {
        isOnPoint = false;
        lastVel = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if (tadpoleData.IsDefending)
        {
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }

    public override void OnFixedUpdate()
    {
        Follow();
    }


    private void Follow()
    {
        if(tadpoleData.FollowTransform != null)
        {
            if (isOnPoint)
            {
                
                transform.position = tadpoleData.FollowTransform.position;
            }
            else
            {
                Vector2 targetPosition = tadpoleData.FollowTransform.position;
                lastVel = Mathf.MoveTowards(lastVel, rotateSpeed.Value, Mathf.Min(accel.Value * Time.fixedDeltaTime, rotateSpeed.Value - lastVel));
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, lastVel * Time.fixedDeltaTime * Mathf.Min(1, Vector2.Distance(transform.position, targetPosition) / 0.5f));
                if(Vector3.Distance(tadpoleData.FollowTransform.position,transform.position) < 0.1f)
                {
                    Debug.Log("Close Enough!");
                    isOnPoint = true;
                }
            }
        }
        else
        {
            tadpoleData.FollowTransform = tadpoleData.TadpoleGroup.GetAFollowPoint();
            rigidbody.velocity = Vector2.zero;
        }

    }

}
