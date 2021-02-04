using UnityEngine;
using System.Collections;



/// <summary>
/// 射程是子弹长度的子弹 
/// </summary>
public class BeamProjecitle : Projectile
{
    private float shootStartTime;
    private float currentLength;
    private bool isLaunched;

    public override void Initialize()
    {
        collider.enabled = false;
        isLaunched = false;
    }

    private void Update()
    {
        if(isLaunched && currentLength < range)
        {
            currentLength = Mathf.MoveTowards(currentLength, range, speed * Time.deltaTime);
            SetBeamLength(currentLength);
        }

        if (Time.time - shootStartTime > lastTime)
        {
            DestroyProjectile();
        }
    }

    public override void Launch(Vector2 position, Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = position;

        collider.enabled = true;

        SetBeamLength(0);
        currentLength = 0;
        isLaunched = true;

        //如果没有预设lasttime，就默认0.3s
        if (lastTime == 0)
        {
            lastTime = Mathf.Max(0.3f, range / speed);
        }

        shootStartTime = Time.time;
    }

    protected override void OnHit(GameObject hitObject, Vector3 hitPos, Vector3 hitDirection)
    {
        HitBehavior hit = hitObject.GetComponent<HitBehavior>();
        Stats stat = hitObject.GetComponent<Stats>();
        if (hit != null && stat != null)
        {
            damage.DealDamage(hit, stat, hitDirection.normalized);
        }
        if (destroyAfterHit) DestroyProjectile();
    }

    private void SetBeamLength(float length)
    {
        spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
        collider.offset = new Vector2(length / 2, 0);

        BoxCollider2D boxCollider = collider.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("The Collider is not boxCollider! Beam must use Box Collider");
            return;
        }
        boxCollider.size = spriteRenderer.size;
    }
}
