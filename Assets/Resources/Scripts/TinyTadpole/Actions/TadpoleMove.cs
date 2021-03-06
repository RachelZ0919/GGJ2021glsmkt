﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TadpoleMove : Action
{
    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;
    private ShootingBehavior shootingBehavior;

    public override void OnAwake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        tadpoleData = GetComponent<TinyTadpole>();
        shootingBehavior = GetComponent<ShootingBehavior>();
    }

    public override void OnStart()
    {
        tadpoleData.IsShooting = true;
        tadpoleData.IsBacking = false;
        tadpoleData.HasHit = false;
    }

    public override TaskStatus OnUpdate()
    {
        transform.parent = tadpoleData.ParentRigidbody.transform.parent;
        rigidbody.velocity = tadpoleData.ShootMovingSpeed * tadpoleData.Direction;
        shootingBehavior.UpdateDirection(tadpoleData.Direction);
        return TaskStatus.Success;
    }

}
