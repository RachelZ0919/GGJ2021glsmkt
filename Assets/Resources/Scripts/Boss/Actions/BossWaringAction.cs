using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class BossWaringAction : Action
{
    public SharedGameObject spriteObject;
    public SharedFloat warningTime;

    private SpriteRenderer sprite;
    private float startTime;

    public override void OnAwake()
    {
        if (spriteObject.Value != null)
        {
            sprite = spriteObject.Value.GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }

    }

    public override void OnStart()
    {
        startTime = Time.time;
        if(sprite != null)  sprite.enabled = true;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime > warningTime.Value)
        {
            if (sprite != null) sprite.enabled = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}