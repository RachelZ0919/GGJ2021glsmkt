using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleGroup : MonoBehaviour
{
    private Queue<TinyTadpole> availableTadpoles; //可用蝌蚪
    private List<TinyTadpole> outTadpoles; //出去的蝌蚪
    private List<TinyTadpole> scatteredTadpoles; //晕倒的蝌蚪
    private TadpoleFollowingPoints points; //跟踪点管理
    private GameObject defenceCircle; //防御圈
    private HitBehavior hit; //受击管理

    [HideInInspector]
    public Vector2 aimingPosition;
    public TinyTadpoleData tadpoleData;
    public Vector2 leadTadpolePosition
    {
        get
        {
            return transform.parent.position;
        }
    }

    private float startDefendTime;
    private float lastDefendTime;
    private bool prepareToDefend;

    private bool getScattered;

    public bool CanDefend
    {
        get
        {
            return !getScattered;
        }
    }

    public bool IsCoolingDown
    {
        get
        {
            return Time.time - lastDefendTime < tadpoleData.defendCooldown;
        }
    }


    private void Awake()
    {
        availableTadpoles = new Queue<TinyTadpole>();
        outTadpoles = new List<TinyTadpole>();
        scatteredTadpoles = new List<TinyTadpole>();
        points = GetComponentInChildren<TadpoleFollowingPoints>();
        defenceCircle = transform.Find("DefenceCircle").gameObject;
        defenceCircle.SetActive(false);
        hit = GetComponentInParent<HitBehavior>();
    }

    private void Start()
    {
        TinyTadpole[] tadpoles = GetComponentsInChildren<TinyTadpole>();

        Rigidbody2D followRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        for (int i = 0; i < tadpoles.Length; i++)
        {
            tadpoles[i].SetData(tadpoleData);
            tadpoles[i].TadpoleGroup = this;
            tadpoles[i].ParentRigidbody = followRigidbody;
            tadpoles[i].FollowTransform = points.AddPoint();
            tadpoles[i].initialized = true;
            availableTadpoles.Enqueue(tadpoles[i]);
        }

        lastDefendTime = Time.time - tadpoleData.defendCooldown;
        startDefendTime = Time.time - tadpoleData.defendTime;
        getScattered = false;
    }

    private void Update()
    {
        if(Time.time - startDefendTime > tadpoleData.defendTime)
        {
            SetShield(false);
        }
    }

    /// <summary>
    /// 随机挑选一个幸运蝌蚪发射
    /// </summary>
    /// <returns>是否发射成功，防御状态不可发射、无蝌蚪可发射状况</returns>
    public bool Shoot()
    {
        if(!IsDefending() && availableTadpoles.Count != 0)
        {
            TinyTadpole tadpole = availableTadpoles.Dequeue();
            
            points.DeletePoint(tadpole.FollowTransform);
            tadpole.FollowTransform = null;

            Vector2 tadpolePosition = tadpole.transform.position;
            Vector2 aimingDirection = aimingPosition - tadpolePosition;
            tadpole.Shoot(aimingDirection.normalized);
            outTadpoles.Add(tadpole);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 召回所有蝌蚪
    /// </summary>
    public void CallbackTadpoles()
    {
        for (int i = 0; i < outTadpoles.Count; i++)
        {
            outTadpoles[i].GoBack();
        }
    }

    /// <summary>
    /// 蝌蚪通知自己回来的函数
    /// </summary>
    /// <param name="tadpole">回来的蝌蚪</param>
    public void Comeback(TinyTadpole tadpole)
    {
        tadpole.transform.parent = transform;
        tadpole.FollowTransform = points.AddPoint();

        //判断是不是在外无法回来的蝌蚪
        if (getScattered)
        {
            bool isInList = scatteredTadpoles.Remove(tadpole);
            if (isInList) //如果是，加入可用蝌蚪，判断是不是可以防御，然后直接返回
            {
                availableTadpoles.Enqueue(tadpole);
                if (scatteredTadpoles.Count == 0)
                {
                    getScattered = false;
                }
                return;
            }
        }

        //如果不是就从在外可回收蝌蚪里去除
        outTadpoles.Remove(tadpole);
        availableTadpoles.Enqueue(tadpole);

        //如果是准备防御状态，看一下是不是满足防御条件
        if (prepareToDefend && outTadpoles.Count == 0)
        {
            prepareToDefend = false;
            Defend();
        }
    }

    /// <summary>
    /// 获得可用蝌蚪数量
    /// </summary>
    /// <returns>可用蝌蚪数量</returns>
    public int GetTadpoleCount()
    {
        return availableTadpoles.Count;
    }


    /// <summary>
    /// 通知要防御
    /// </summary>
    public void ActivateDefence()
    {
        if(Time.time - lastDefendTime >= tadpoleData.defendCooldown) //如果过了CD
        {
            lastDefendTime = Time.time; //刷新时间

            if (!getScattered) //如果是分散状态
            {
                if (outTadpoles.Count == 0)
                {
                    Defend();
                }
                else
                {
                    prepareToDefend = true;
                    CallbackTadpoles();
                }
            }
            else
            {
                CallbackTadpoles(); //除了召回什么也不做
            }
        }
    }

    /// <summary>
    /// 真的开始防御
    /// </summary>
    private void Defend()
    {
        for(int i = 0; i < availableTadpoles.Count; i++)
        {
            TinyTadpole tadpole = availableTadpoles.Dequeue();
            tadpole.Defend();
            availableTadpoles.Enqueue(tadpole);
        }
        
        SetShield(true);
    }

    /// <summary>
    /// 判断是否在防御
    /// </summary>
    /// <returns></returns>
    public bool IsDefending()
    {
        return prepareToDefend || Time.time - startDefendTime <= tadpoleData.defendTime;
    }

    /// <summary>
    /// 获得一个跟踪点
    /// </summary>
    /// <returns></returns>
    public Transform GetAFollowPoint()
    {
        return points.AddPoint();
    }

    public void ForceScatter(Transform scatterPointParent)
    {
        ResetShield();

        int pointIdx = 0;

        //如果是在外的，就直接配个点，散了
        for (int i = 0; i < outTadpoles.Count; i++, pointIdx++)  
        {
            outTadpoles[i].FollowTransform = scatterPointParent.GetChild(pointIdx);
            outTadpoles[i].ForceScatter();
            scatteredTadpoles.Add(outTadpoles[i]);
            Debug.Log("out:" + outTadpoles[i].gameObject.name);
        }

        outTadpoles.Clear();

        int totalAvailableCount = availableTadpoles.Count;

        //如果是可用的，删掉原有的跟踪点，再通知散开
        for (int i = 0; i < totalAvailableCount; i++, pointIdx++) 
        {

            TinyTadpole tadpole = availableTadpoles.Dequeue();
            Debug.Log(tadpole.gameObject.name);
            points.DeletePoint(tadpole.FollowTransform);
            tadpole.FollowTransform = scatterPointParent.GetChild(pointIdx);
            tadpole.ForceScatter();

            scatteredTadpoles.Add(tadpole);
        }
        
        getScattered = true;
    }

    /// <summary>
    /// 设置护盾
    /// </summary>
    /// <param name="isOpen">开还是关</param>
    private void SetShield(bool isOpen)
    {
        defenceCircle.SetActive(isOpen);
        hit.shieldOpen = isOpen;
        if (isOpen)
        {
            points.MoveInCircle(tadpoleData.defendTime);
            startDefendTime = Time.time;

        }
    }

    /// <summary>
    /// 重置护盾
    /// </summary>
    private void ResetShield()
    {
        SetShield(false);
        prepareToDefend = false;
        startDefendTime = Time.time - tadpoleData.defendTime - 0.1f;
    }
}
