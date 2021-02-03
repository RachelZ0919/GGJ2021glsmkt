using UnityEngine;
using System.Collections;


/// <summary>
/// 武器基本数据
/// </summary>
public class WeaponData : ScriptableObject
{
    /// <summary>
    /// 子弹对象池名字
    /// </summary>
    public string projectilePoolName;

    /// <summary>
    /// 弹夹子弹数量
    /// </summary>
    public int projectilesPerClip;

    /// <summary>
    /// 冷却时间
    /// </summary>
    public float cooldownTime;

    /// <summary>
    /// 武器攻击力
    /// </summary>
    public float attack;

    /// <summary>
    /// 子弹速度
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// 射击时间间隔
    /// </summary>
    public float shootingInter;

    /// <summary>
    /// 射击距离
    /// </summary>
    public float range;

    /// <summary>
    /// 持续时间，如果是-1就说明按射程计算
    /// </summary>
    public float lastTime;

    /// <summary>
    /// 后坐力大小
    /// </summary>
    public float recoilForce;

    /// <summary>
    /// 击退强度
    /// </summary>
    public float knockbackForce;

    /// <summary>
    /// 子弹类型
    /// </summary>
    public ProjectileData projectile;

    /// <summary>
    /// 射击音效
    /// </summary>
    public AudioClip shootingAudio;
}
