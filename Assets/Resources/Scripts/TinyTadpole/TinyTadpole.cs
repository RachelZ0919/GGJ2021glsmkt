using UnityEngine;
using BehaviorDesigner.Runtime;


public class TinyTadpole : MonoBehaviour
{
    private BehaviorTree behaviorTree;
    private TinyTadpoleData tadpoleData;
    [SerializeField] private AudioManager audio;
    [SerializeField] private AudioClip hitSound;

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
                return 100;
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

    #region HitDamage

    public bool IsShooting { get; set; }
    public bool IsBacking { get; set; }
    public bool HasHit { get; set; }
    public bool IsSleeping { get; set; }
    Damage shootDamage;
    Damage backDamage;

    #endregion


    private void Awake()
    {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public void SetData(TinyTadpoleData data)
    {
        tadpoleData = data;

        shootDamage = new NormalDamage();
        shootDamage.damage = tadpoleData.hitAttack;
        shootDamage.knockbackForce = 0;
        backDamage = new NormalDamage();
        backDamage.damage = tadpoleData.backHitAttack;
        shootDamage.knockbackForce = 0;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitBehavior hit = collision.gameObject.GetComponent<HitBehavior>();
            if (!HasHit)
            {
                if (IsShooting)
                {
                    HasHit = true;
                    shootDamage.DealDamage(hit, null, Direction, false);
                    if (audio != null) audio.PlayAudio(hitSound);
                }else if (IsBacking)
                {
                    HasHit = true;
                    Vector2 myPos = transform.position;
                    backDamage.DealDamage(hit, null, (ParentRigidbody.position - myPos).normalized, false);
                }
            }
        }else if (collision.gameObject.CompareTag("Player"))
        {
            GoBack();
        }
    }

    public void ForceScatter()
    {
        if (initialized)
        {
            behaviorTree.SendEvent("ForceSleep");
        }
    }
}
