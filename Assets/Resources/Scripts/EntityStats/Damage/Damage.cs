using UnityEngine;



    public enum DamageType
    {
        NormalDamage = 1
    }

/// <summary>
/// 伤害类型基类
/// </summary>
abstract public class Damage
{
    /// <summary>
    /// 基础伤害
    /// </summary>
    public float damage { get; set; }
    /// <summary>
    /// 击退强度
    /// </summary>
    public float knockbackForce { get; set; }
    /// <summary>
    /// 根据stat计算实际伤害，将异常状态和实际伤害传输给受击行为
    /// </summary>
    /// <param name="stat">人物属性</param>
    /// <param name="hitBehavior">受击后行为</param>
    /// <param name="direction">受击方向，只有x轴</param>
    abstract public void DealDamage(HitBehavior hitBehavior, Stats stat, Vector2 direction, bool isTouchDamage = false);

    static public Damage GetDamage(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.NormalDamage:
                {
                    return new NormalDamage();
                }
        }
        return null;
    }
}

