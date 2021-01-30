using UnityEngine;
using System.Collections.Generic;
using System;


/// <summary>
/// 特效对象池，有哪些池子暂时在inspector内提前设置
/// </summary>
public class EffectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }
    static public EffectPool instance;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    //临时先在inspector里提前配置好，都是dontdestroyonload类型
    public List<Pool> poolInfos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        InitializePools();
    }

    //初始化pool
    private void InitializePools()
    {
        for (int i = 0; i < poolInfos.Count; i++)
        {
            Pool poolInfo = poolInfos[i];
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int j = 0; j < poolInfo.size; j++)
            {
                GameObject obj = Instantiate(poolInfo.prefab);

                obj.GetComponent<EffectManager>().poolName = poolInfo.name;
                obj.SetActive(false);
                DontDestroyOnLoad(obj);

                pool.Enqueue(obj);
            }
            poolDictionary.Add(poolInfo.name, pool);
        }
    }


    /// <summary>
    /// 添加特效
    /// </summary>
    /// <param name="effectName">特效名</param>
    /// <param name="size">池子大小</param>
    /// <param name="effect">特效</param>
    public void AddEffect(string effectName, int size, GameObject effect)
    {
        if (poolDictionary.ContainsKey(effectName))
        {
            Debug.LogWarning("The pool" + effectName + "already exists.");
            return;
        }

        Queue<GameObject> pool = new Queue<GameObject>();
        for (int j = 0; j < size; j++)
        {
            GameObject obj = Instantiate(effect);

            obj.GetComponent<EffectManager>().poolName = effectName;
            obj.SetActive(false);
            DontDestroyOnLoad(obj);
            pool.Enqueue(obj);
        }
        poolDictionary.Add(effectName, pool);
    }


    /// <summary>
    /// 在指定位置播放指定特效
    /// </summary>
    /// <param name="name">特效名</param>
    /// <param name="position">特效位置</param>
    /// <param name="direction">特效方向</param>
    public void PlayEffect(string name, Vector2 position, Vector2 direction)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool" + name + "does not exist!");
        }

        Queue<GameObject> queue = poolDictionary[name];
        GameObject effect = queue.Dequeue();

        //如果不剩了加三个
        if (queue.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(effect);
                obj.GetComponent<EffectManager>().poolName = name;
                obj.SetActive(false);
                DontDestroyOnLoad(obj);
                queue.Enqueue(obj);
            }
        }

        //播放特效
        effect.SetActive(true);
        effect.GetComponent<EffectManager>().PlayEffect(position, direction);
    }

    /// <summary>
    /// 回收特效
    /// </summary>
    /// <param name="name">特效名</param>
    /// <param name="effect">对象</param>
    public void RecycleEffect(string name, GameObject effect)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool" + name + "does not exist!");
        }

        effect.SetActive(false);
        poolDictionary[name].Enqueue(effect);
    }
}
