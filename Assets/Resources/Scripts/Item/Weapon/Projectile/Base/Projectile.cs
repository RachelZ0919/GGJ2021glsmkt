using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 子弹基类
/// </summary>
abstract public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 应该发生碰撞的实体layer
    /// </summary>
    public LayerMask layermaskToHit { get; set; }
    /// <summary>
    /// 子弹击中后理应造成的伤害
    /// </summary>
    public Damage damage { get; set; }
    /// <summary>
    /// 子弹速度
    /// </summary>
    public float speed { get; set; }
    /// <summary>
    /// 子弹射程
    /// </summary>
    public float range { get; set; }
    /// <summary>
    /// 子弹持续时间
    /// </summary>
    public float lastTime { get; set; }
    /// <summary>
    /// 所属对象池
    /// </summary>
    public string poolName { protected get; set; }
    public bool destroyAfterHit { protected get; set; }

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody;
    [SerializeField] public Collider2D collider;


    public bool isDead;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //判断是不是在应该撞的layer
        if (!isDead && ((1 << collision.gameObject.layer) & layermaskToHit) > 0)
        {
            Vector3 velocity = rigidbody.velocity.normalized;
            Vector3 hitEffectPosition = transform.position + velocity * 0.1f;
            OnHit(collision.gameObject, hitEffectPosition, -velocity);
        }
    }


    /// <summary>
    /// 初始化子弹行为
    /// </summary>
    abstract public void Initialize();

    /// <summary>
    /// 发射子弹，发射时参数设置override这个函数
    /// </summary>
    /// <param name="direction">发射方向</param>
    /// <param name="position">发射起点</param>
    abstract public void Launch(Vector2 position, Vector2 direction);

    /// <summary>
    /// 碰撞时调用，击中后行为override这个函数
    /// </summary>
    /// <param name="hitObject">碰撞物体</param>
    abstract protected void OnHit(GameObject hitObject, Vector3 hitPos, Vector3 hitDirection);

    /// <summary>
    /// 子弹销毁时调用
    /// </summary>
    protected void DestroyProjectile()
    {
        ProjectilePool.instance.RecycleProjectile(poolName, this); //回收
    }
}
