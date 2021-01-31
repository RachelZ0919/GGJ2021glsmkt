using UnityEngine;
using System;
using System.Collections.Generic;


/// <summary>
/// 人物属性类
/// </summary>
public class Stats : MonoBehaviour
{
    #region Stats
    /// <summary>
    /// HP
    /// </summary>
    public float health
    {
        get
        {
            if (stats != null && stats.ContainsKey("health"))
            {
                return stats["health"];
            }
            else
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 最大生命值
    /// </summary>
    public float maxHealth
    {
        get
        {
            if (stats != null && stats.ContainsKey("maxHealth"))
            {
                return stats["maxHealth"];
            }
            else
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 击退抗性
    /// </summary>
    public float knockBackResist
    {
        get
        {
            if (stats != null && stats.ContainsKey("knockbackResist"))
            {
                return stats["knockbackResist"];
            }
            else
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 属性字典
    /// </summary>
    private Dictionary<string, float> stats;
    #endregion

    #region Observer interface
    public delegate void StatsChanged(Stats stat);
    public StatsChanged OnStatsChanged;
    #endregion

    private bool isInitialized = false;
    public StatData statData;

    private void Awake()
    {
        stats = new Dictionary<string, float>();
    }

    private void Start()
    {
        if (statData != null)
        {
            InitializeStats(statData);
        }
        else
        {
            Debug.LogError("No Stat Data!");
        }
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="value"></param>
    public void SetValue(string name, float value)
    {
        if (stats.ContainsKey(name))
        {
            stats[name] = value;
        }
        OnStatsChanged?.Invoke(this);
    }

    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="name">属性名</param>
    /// <param name="value">值</param>
    public void AddProperty(string name, float value)
    {
        if (stats.ContainsKey(name))
        {
            stats[name] = value;
        }
        else
        {
            stats.Add(name, value);
        }
        if(isInitialized) OnStatsChanged?.Invoke(this);
    }

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="name">属性名</param>
    /// <returns>属性值</returns>
    public float GetProperty(string name)
    {
        if (stats.ContainsKey(name))
        {
            return stats[name];
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 清空属性
    /// </summary>
    public void ClearProperty()
    {
        isInitialized = false;
        if (stats != null) stats.Clear();
        else stats = new Dictionary<string, float>();
    }

    /// <summary>
    /// 初始化所有属性
    /// </summary>
    public void InitializeStats(StatData statData)
    {
        ClearProperty();
        if (stats == null) stats = new Dictionary<string, float>();
        AddProperty("health", statData.health);
        AddProperty("maxHealth", statData.health);
        isInitialized = true;
        OnStatsChanged?.Invoke(this);
    }

}

