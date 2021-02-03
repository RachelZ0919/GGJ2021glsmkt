using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;


public class ForceLookAtBossAction : Action
{
    public SharedGameObject shootingObject;
    public SharedFloat totalLastTime;
    public SharedFloat allowLookedAwayTime;

    public SharedGameObject warningAnimationObject;

    private TadpoleGroup tadpoles;
    private ShootingBehavior shootingBehavior;
    private UnityArmatureComponent anim;

    private float lastLookedAwayTime;
    private float startTime;

    public override void OnAwake()
    {
        GameObject tadpole = GameObject.FindGameObjectWithTag("Player");
        tadpoles = tadpole.transform.parent.GetComponentInChildren<TadpoleGroup>();
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
    }

    public override void OnStart()
    {
        startTime = lastLookedAwayTime = Time.time;
        anim.animation.Play("beam_attack");
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= totalLastTime.Value)
        {
            anim.animation.Play("idle");
            return TaskStatus.Success;
        }

        Vector2 startPoint = tadpoles.leadTadpolePosition;
        Vector2 direction = tadpoles.aimingPosition - startPoint;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, LayerMask.GetMask("LookAtRegion"));
        if(hit.collider != null)
        {
            lastLookedAwayTime = Time.time;
        }

        if(Time.time - lastLookedAwayTime >= allowLookedAwayTime.Value)
        {
            Vector2 startPos = shootingBehavior.transform.position;
            Vector2 shootingDirection = tadpoles.leadTadpolePosition - startPos;
            shootingBehavior.setting.range = 50;
            shootingBehavior.Shoot(shootingDirection);

            lastLookedAwayTime = Time.time;
        }

        //todo:视觉表现


        return TaskStatus.Running;
    }

}
