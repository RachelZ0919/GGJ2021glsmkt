using UnityEngine;
using BehaviorDesigner.Runtime;


public class TinyTadpole : MonoBehaviour
{
    private BehaviorTree behaviorTree;
    private TinyTadpoleData tadpoleData;

    [HideInInspector]
    public bool isShoot = false;
    [HideInInspector]
    public bool initialized = false;

    #region properties
    public Vector2 Direction { get; private set; }
    public float ShootMovingSpeed
    {
        get
        {
            return tadpoleData.shootMovingSpeed;
        }
    }
    public float CallbackSpeed
    {
        get
        {
            return tadpoleData.callbackSpeed;
        }
    }
    public float FollowSpeed
    {
        get
        {
            return tadpoleData.followSpeed;
        }
    }
    public float ShootAttack
    {
        get
        {
            return tadpoleData.baseShootingAttack;
        }
    }
    public float HitAttack
    {
        get
        {
            return tadpoleData.hitAttack;
        }
    }
    public float ShootingInter
    {
        get
        {
            int tadpoleCount = TadpoleGroup.GetTadpoleCount();
            if(tadpoleCount == 0)
            {
                return 0;
            }
            else
            {
                return tadpoleData.baseShootingInter * tadpoleData.shootSpeedBuffs[Mathf.Min(4, tadpoleCount) - 1];
            }
            
        }
    }
    public Rigidbody2D ParentRigidbody { get; set; }
    public Transform FollowTransform { get; set; }
    public TadpoleGroup TadpoleGroup { get; set; }
    public bool IsDefending
    {
        get
        {
            return TadpoleGroup.IsDefending();
        }
    }

    #endregion


    private void Awake()
    {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public void SetData(TinyTadpoleData data)
    {
        tadpoleData = data;
    }

    public void Shoot(Vector2 direction)
    {
        if (initialized)
        {
            Direction = direction;
            behaviorTree.SendEvent("Shoot");
        }
    }

    public void GoBack()
    {
        if (initialized)
        {
            behaviorTree.SendEvent("Callback");
        }
    }

    public void Defend()
    {
        if (initialized)
        {
            behaviorTree.SendEvent("Defend");
        }

    }
}
