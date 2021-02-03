using UnityEngine;
using System.Collections;


public class BoomerangProjectile : Projectile
{
    //range是一阶段运动距离，lastTime是停顿时间
    private Vector2 startPosition;
    private Vector2 oriDirection;
    private bool isLaunched = false;
    private bool isWaitingToGoBack;
    private float waitingStartTime;

    public override void Initialize()
    {
        rigidbody.velocity = Vector2.zero;
        isLaunched = isWaitingToGoBack = false;
    }

    private void Update()
    {
        if (isWaitingToGoBack)
        {
            if(Time.time - waitingStartTime >= lastTime)
            {
                rigidbody.velocity = -oriDirection;
                isWaitingToGoBack = false;
            }

        }else if(isLaunched)
        {
            float distance = Vector2.Distance(startPosition, rigidbody.position);
            Vector2 targetVel = oriDirection * Mathf.Min(1, distance / 3 + 0.1f);
            rigidbody.velocity = targetVel;

            if (Mathf.Abs(distance - range) < 0.1f)
            {
                rigidbody.position = startPosition + oriDirection.normalized * range;
                isWaitingToGoBack = true;
                rigidbody.velocity = Vector2.zero;
                waitingStartTime = Time.time;
                isLaunched = false;

            }
        }
    }

    public override void Launch(Vector2 position, Vector2 direction)
    {
        isLaunched = true;
        collider.enabled = true;

        //设置子弹速度
        rigidbody.velocity = oriDirection = direction.normalized * speed;

        //纪录开始位置
        startPosition = position;

        //设置子弹角度
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, angle);

        //设置子弹位置
        transform.position = position;
    }

    protected override void OnHit(GameObject hitObject, Vector3 hitPos, Vector3 hitDirection)
    {
        HitBehavior hit = hitObject.GetComponent<HitBehavior>();
        Stats stat = hitObject.GetComponent<Stats>();
        if (hit != null && stat != null)
        {
            damage.DealDamage(hit, stat, hitDirection.normalized);
        }
        collider.enabled = false;
        EffectPool.instance?.PlayEffect("hit_effect", hitPos, hitDirection);
        if(destroyAfterHit) DestroyProjectile();
    }

}
