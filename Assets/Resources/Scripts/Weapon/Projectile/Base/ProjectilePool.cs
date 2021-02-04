using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


/// <summary>
/// 子弹对象池，负责管理子弹对象的生成与回收。singleton，且切换场景时不会销毁。
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    private struct Pool
    {
        public ProjectileData projectileData;
        public Queue<Projectile> pool;
    }
    static public ProjectilePool instance;
    private Dictionary<string, Pool> poolDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            poolDictionary = new Dictionary<string, Pool>();
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// 增加新的子弹池，如果user已存在，把子弹池扩充到size大小。
    /// 不会检查池里的子弹是否是同一类子弹。
    /// </summary>
    /// <param name="user">使用子弹池的对象，可以是枪，可以是一类敌人。</param>
    /// <param name="projectile">子弹Prefab</param>
    /// <param name="size">池的大小</param>
    /// <param name="destroyOnLoad">子弹是否在场景切换时销毁</param>
    public void AddPool(string user, ProjectileData projectile, int size)
    {
        if (poolDictionary.ContainsKey(user))
        {
            Pool pool = poolDictionary[user];
            Queue<Projectile> projectilePool = poolDictionary[user].pool;
        }
        else
        {
            //创建一个新的对象池
            Queue<Projectile> projectilePool = new Queue<Projectile>();
            Pool pool = new Pool();
            pool.projectileData = projectile;
            for (int i = 0; i < size; i++)
            {
                Projectile proj = projectile.GenerateProjectile();
                proj.gameObject.name = user + "_projectile" + $"_{i}";
                proj.poolName = user;
                proj.gameObject.SetActive(false);
                projectilePool.Enqueue(proj);
            }
            pool.pool = projectilePool;
            poolDictionary.Add(user, pool);
        }
    }

    /// <summary>
    /// 获取对象池的子弹
    /// </summary>
    /// <param name="user">使用对象池的对象</param>
    /// <returns>子弹对象</returns>
    public Projectile SpawnAProjectile(string user)
    {
        //检查有没有这个key
        if (!poolDictionary.ContainsKey(user))
        {
            Debug.LogWarning(user + "doesn't have a projectile pool.");
            return null;
        }

        Pool pool = poolDictionary[user];
        Queue<Projectile> projectiles = pool.pool;

        //如果没有子弹，加五个进去
        if (projectiles.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                Projectile proj = pool.projectileData.GenerateProjectile();
                proj.poolName = user;
                proj.gameObject.name = user + "_projectile";
                proj.gameObject.SetActive(false);
                pool.pool.Enqueue(proj);
            }
        }

        //获取并初始化子弹

        Projectile projectile = poolDictionary[user].pool.Dequeue();

        projectile.gameObject.SetActive(true);
        projectile.Initialize();
        return projectile;

    }

    /// <summary>
    /// 回收子弹对象，并SetActive（false)
    /// </summary>
    /// <param name="user">使用子弹池的对象</param>
    /// <param name="projectile">子弹</param>
    public void RecycleProjectile(string user, Projectile projectile)
    {
        //检查有没有这个key
        if (!poolDictionary.ContainsKey(user))
        {
            Debug.LogError("There is no projectile pool belongs to " + user);
            return;
        }

        //回收并deactivate
        if (projectile.gameObject.activeSelf)
        {
            poolDictionary[user].pool.Enqueue(projectile);
            projectile.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 删除子弹池
    /// </summary>
    /// <param name="user">使用子弹池的对象</param>
    public void DeletePool(string user)
    {
        if (!poolDictionary.ContainsKey(user))
        {
            Debug.LogWarning("There is no projectile pool belongs to" + user);
            return;
        }

        //删除子弹池
        Queue<Projectile> projectiles = poolDictionary[user].pool;
        while (projectiles.Count > 0)
        {
            Projectile projectile = projectiles.Dequeue();
            if (projectile != null) Destroy(projectile.gameObject);
        }
        poolDictionary.Remove(user);

    }
}
