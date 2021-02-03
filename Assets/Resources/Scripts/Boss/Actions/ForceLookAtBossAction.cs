using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class ForceLookAtBossAction : Action
{
    public SharedFloat totalLastTime;
    public SharedFloat allowLookedAwayTime;
    public SharedFloat damageAmount;

    public SharedGameObject warningAnimationObject;

    private TadpoleGroup tadpoles;
    private HitBehavior hitBehavior;
    private ForceDamage damage;

    private float lastLookedAwayTime;
    private float startTime;

    public override void OnAwake()
    {
        GameObject tadpole = GameObject.FindGameObjectWithTag("Player");
        tadpoles = tadpole.GetComponentInChildren<TadpoleGroup>();
        hitBehavior = tadpole.GetComponent<HitBehavior>();
    }

    public override void OnStart()
    {
        startTime = lastLookedAwayTime = Time.time;
        damage = new ForceDamage();
        damage.damage = damageAmount.Value;
        damage.knockbackForce = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= totalLastTime.Value)
        {
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
            damage.DealDamage(hitBehavior, null, Vector2.zero);
            lastLookedAwayTime = Time.time;
        }

        Debug.Log($"Boss Hit Left Time: {allowLookedAwayTime.Value - (Time.time - lastLookedAwayTime)}");

        //todo:视觉表现


        return TaskStatus.Running;
    }

}
