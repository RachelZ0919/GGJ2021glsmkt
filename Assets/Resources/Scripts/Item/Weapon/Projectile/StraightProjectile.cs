using UnityEngine;
using System.Collections;


/// <summary>
/// 普通子弹
/// </summary>
public class StraightProjectile : Projectile
{
    private float startTime;
    private bool isLaunched = false;

    private void Update()
    {
        //超过射程回收子弹
        if (Time.time - startTime >= lastTime)
        {
            //todo:消失时动画
            DestroyProjectile();
        }
    }

    public override void Initialize()
    {
        rigidbody.velocity = Vector2.zero;
        isLaunched = true;
    }

    public override void Launch(Vector2 position, Vector2 direction)
    {
        //Debug.Log($"launch{name}");
        //已经发射
        isLaunched = true;
        collider.enabled = true;

        //记录开始时间
        startTime = Time.time;
        if (lastTime == 0) lastTime = range / speed;

        //设置子弹速度
        rigidbody.velocity = direction.normalized * speed;

        //设置子弹角度
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, angle);

        //设置子弹位置
        transform.position = position;
    }

    protected override void OnHit(GameObject hitObject, Vector3 hitPos, Vector3 hitDirection)
    {
        //Debug.Log($"hit {hitObject.name}");
        HitBehavior hit = hitObject.GetComponent<HitBehavior>();
        Stats stat = hitObject.GetComponent<Stats>();
        if (hit != null && stat != null)
        {
            damage.DealDamage(hit, stat, hitDirection.normalized);
        }
        collider.enabled = false;
        EffectPool.instance?.PlayEffect("hit_effect", hitPos, hitDirection);
        DestroyProjectile();
    }
}
