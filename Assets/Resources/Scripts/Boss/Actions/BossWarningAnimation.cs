﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossWarningAnimation : Action
{
    public SharedGameObject animationObject;
    public SharedFloat warningTime;

    private float startTime;


    public override void OnStart()
    {
        animationObject.Value.SetActive(true);
        startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= warningTime.Value)
        {
            animationObject.Value.SetActive(false);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
