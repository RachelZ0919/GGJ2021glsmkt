using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;



public class BossKillerBeamAction : Action
{
    public SharedGameObject shootingObject;
    public SharedTransform aimingTransform;

    private ShootingBehavior shootingBehavior;

    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
    }

    public override TaskStatus OnUpdate()
    {
        float length = Vector3.Distance(aimingTransform.Value.position, shootingBehavior.holdingPoint.position);
        shootingBehavior.setting.range = length;
        Debug.Log(shootingBehavior.setting.range);
        Vector2 targetPosition = aimingTransform.Value.position - shootingBehavior.holdingPoint.position;
        if (shootingBehavior.Shoot(targetPosition))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }


    }

}