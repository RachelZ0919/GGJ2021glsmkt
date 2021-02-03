using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleGroup : MonoBehaviour
{
    public TinyTadpoleData tadpoleData;

    private Queue<TinyTadpole> availableTadpoles; //可用蝌蚪
    private List<TinyTadpole> outTadpoles; //出去的蝌蚪
    private List<TinyTadpole> scatteredTadpoles; //晕倒的蝌蚪
    private TadpoleFollowingPoints points; //跟踪点管理
    private GameObject defenceCircle; //防御圈
    private HitBehavior hit; //受击管理

    private Transform playerTransform;
    private Vector3 positionOffset;
    [HideInInspector] public Vector2 aimingPosition;
    /// <summary>
    /// 玩家操纵蝌蚪所在位置
    /// </summary>
    public Vector2 leadTadpolePosition
    {
        get
        {
            return playerTransform.position;
        }
    }

    #region Defend

    private float startDefendTime;//该次防御开始时间
    private float lastDefendTime;//上次开始防御时间（计算CD用）
    private bool prepareToDefend;//是否在等待僚机回归并开启防御
    private bool getScattered;//有没有被强制散开
    /// <summary>
    /// 是否可以触发防御
    /// </summary>
    public bool CanDefend
    {
        get
        {
            return !getScattered;
        }
    }
    /// <summary>
    /// 是否在冷却防御技能
    /// </summary>
    public bool IsCoolingDown
    {
        get
        {
            return Time.time - lastDefendTime < tadpoleData.defendCooldown;
        }
    }

    #endregion

    #region Audio

    [SerializeField] private AudioManager audio;
    [SerializeField] private AudioClip shootSoundEffect;
    [SerializeField] private AudioClip defendSoundEffect;
    
    #endregion

    private void Awake()
    {
        availableTadpoles = new Queue<TinyTadpole>();
        outTadpoles = new List<TinyTadpole>();
        scatteredTadpoles = new List<TinyTadpole>();

        points = GetComponentInChildren<TadpoleFollowingPoints>();

        defenceCircle = transform.Find("DefenceCircle").gameObject;
        defenceCircle.SetActive(false);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        positionOffset = transform.position - playerTransform.position;

        hit = playerTransform.GetComponent<HitBehavior>();
    }

    private void Start()
    {
        TinyTadpole[] tadpoles = GetComponentsInChildren<TinyTadpole>();

        Rigidbody2D followRigidbody = playerTransform.GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
        transform.position = playerTransform.position + positionOffset;
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

            if (audio != null) audio.PlayAudio(shootSoundEffect);

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
            if (!getScattered) //如果是分散状态
            {
                lastDefendTime = Time.time; //刷新时间
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

        if (audio != null) audio.PlayAudio(defendSoundEffect);
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
            tadpole.transform.parent = transform.parent;
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
