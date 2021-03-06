﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;




public class BossKillerBeamAction : Action
{
    public SharedGameObject shootingObject;
    private Transform aimingTransform;

    private ShootingBehavior shootingBehavior;

    public override void OnAwake()
    {
        aimingTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStart()
    {
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
    }

    public override TaskStatus OnUpdate()
    {
        shootingBehavior.setting.range = 50;
        Vector2 targetPosition = aimingTransform.position - shootingBehavior.holdingPoint.position;
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