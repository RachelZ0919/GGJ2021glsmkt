using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    /// <summary>
    /// 所属对象池名字
    /// </summary>
    [HideInInspector] public string poolName;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 初始化特效
    /// </summary>
    private void Initialize()
    {

    }

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="position">播放位置</param>
    /// <param name="direction">特效的方向</param>
    public void PlayEffect(Vector2 position, Vector2 direction)
    {
        Initialize();
        transform.position = new Vector3(position.x, position.y, 0);
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// 特效播放结束时调用函数
    /// </summary>
    public void OnPlayEnd()
    {
        EffectPool.instance.RecycleEffect(poolName, gameObject);
    }
}
