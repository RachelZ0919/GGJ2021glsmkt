﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;


public class TadpoleSleep : Action
{
    public SharedGameObject animationObject;
    private UnityArmatureComponent unityArmature;
    private Rigidbody2D rigidbody;
    private TinyTadpole tadpoleData;

    public override void OnAwake()
    {
        tadpoleData = GetComponent<TinyTadpole>();
        rigidbody = GetComponent<Rigidbody2D>();
        unityArmature = animationObject.Value.GetComponent<UnityArmatureComponent>();
    }

    public override TaskStatus OnUpdate()
    {
        tadpoleData.IsShooting = false;
        tadpoleData.HasHit = true;
        rigidbody.velocity = Vector2.zero;
        unityArmature.animation.Play("newAnimation_1");
        return TaskStatus.Success;
    }
}
