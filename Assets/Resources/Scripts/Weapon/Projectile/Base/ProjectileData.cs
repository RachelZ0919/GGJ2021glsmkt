using UnityEngine;



public enum ProjectileType
{
    Straight = 1,
    Beam = 2,
    Boomerang = 3
}

public enum ColliderType
{
    Circle = 1,
    Box = 2
}

/// <summary>
/// 用来生成子弹的数据，在WeaponData内，但projectile本身不用这些数据。
/// </summary>
public class ProjectileData : ScriptableObject
{
    [HideInInspector] public GameObject projectilePrefab;
    /// <summary>
    /// 子弹是否是动画
    /// </summary>
    public bool isAnim;
    /// <summary>
    /// 子弹样式
    /// </summary>
    public Sprite projectileImage;
    /// <summary>
    /// 子弹动画样式
    /// </summary>
    public GameObject projectileAnim;
    /// <summary>
    /// 子弹伤害类型
    /// </summary>
    public DamageType damageType;
    /// <summary>
    /// 子弹类型
    /// </summary>
    public ProjectileType type;
    /// <summary>
    /// 子弹碰撞体类型
    /// </summary>
    public ColliderType colliderType;
    /// <summary>
    /// 子弹大小
    /// </summary>
    public float size;
    /// <summary>
    /// 击中敌人后是否消失
    /// </summary>
    public bool destroyAfterHit;

    public Projectile GenerateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab);

        //设置图片
        Transform spriteTransform = projectile.transform.Find("sprite");
        SpriteRenderer projectileSprite = spriteTransform.GetComponent<SpriteRenderer>();

        //设置碰撞体
        Collider2D collider = null;

        if (isAnim)
        {
            projectileSprite.enabled = false;
            GameObject animObj = Instantiate(projectileAnim, spriteTransform);
            collider = animObj.GetComponentInChildren<Collider2D>();
        }
        else
        {
            projectileSprite.sprite = projectileImage;
            projectileSprite.sortingOrder = 1;
            switch (colliderType)
            {
                case ColliderType.Circle:
                    {
                        CircleCollider2D circleCollider = spriteTransform.gameObject.AddComponent<CircleCollider2D>();
                        collider = circleCollider;
                        break;
                    }
                case ColliderType.Box:
                    {
                        BoxCollider2D boxCollider = spriteTransform.gameObject.AddComponent<BoxCollider2D>();
                        collider = boxCollider;
                        break;
                    }
            }
        }

        collider.isTrigger = true;

        //设置子弹大小
        spriteTransform.localScale = new Vector3(size, size, 1);


        switch (type)
        {
            case ProjectileType.Straight:
                {
                    Projectile proj = projectile.AddComponent<StraightProjectile>();
                    proj.damage = Damage.GetDamage(damageType);
                    proj.collider = collider;
                    proj.destroyAfterHit = destroyAfterHit;
                    return proj;
                }
            case ProjectileType.Beam:
                {
                    Projectile proj = projectile.AddComponent<BeamProjecitle>();
                    proj.damage = Damage.GetDamage(damageType);
                    proj.collider = collider;
                    projectileSprite.drawMode = SpriteDrawMode.Sliced;
                    proj.destroyAfterHit = destroyAfterHit;
                    return proj;
                }
            case ProjectileType.Boomerang:
                {
                    Projectile proj = projectile.AddComponent<BoomerangProjectile>();
                    proj.damage = Damage.GetDamage(damageType);
                    proj.collider = collider;
                    proj.destroyAfterHit = destroyAfterHit;
                    return proj;
                }
        }
        return null;
    }


}
