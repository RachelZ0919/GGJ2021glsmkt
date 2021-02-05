using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DragonBones;


public class ForceLookAtBossAction : Action
{
    public SharedGameObject shootingObject;
    public SharedFloat totalLastTime;
    public SharedFloat allowLookedAwayTime;

    public SharedGameObject warningAnimationObject;

    public SharedGameObject audioObject;

    private TadpoleGroup tadpoles;
    private ShootingBehavior shootingBehavior;
    private UnityArmatureComponent anim;
    private UnityArmatureComponent warningAnim;
    private string warningAnimName;

    private float lastLookedAwayTime;
    private float startTime;
    private bool hasStartWarning;
    private float warningFade;

    private float scaleTime;
    private float blinkTime;
    private float blinkInter;

    private AudioManager audio;

    public override void OnAwake()
    {
        GameObject tadpole = GameObject.FindGameObjectWithTag("Player");
        tadpoles = tadpole.transform.parent.GetComponentInChildren<TadpoleGroup>();
        shootingBehavior = shootingObject.Value.GetComponent<ShootingBehavior>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();

        scaleTime = allowLookedAwayTime.Value * 0.6f;
        blinkTime = allowLookedAwayTime.Value * 0.4f;
        blinkInter = blinkTime * 0.25f;

        if (audioObject.Value != null) audio = audioObject.Value.GetComponent<AudioManager>();
        else Debug.LogError("Audio Manager Object is not assigned yet!");
    }

    public override void OnStart()
    {
        startTime = lastLookedAwayTime = Time.time;
        anim.animation.Play("beam_attack");

        warningAnimationObject.Value.SetActive(true);
        warningAnim = warningAnimationObject.Value.GetComponent<UnityArmatureComponent>();
        warningAnimName = warningAnim.animation.lastAnimationName;
        warningAnimationObject.Value.SetActive(false);

        hasStartWarning = false;
        warningFade = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if(Time.time - startTime >= totalLastTime.Value)
        {
            anim.animation.Play("idle");
            return TaskStatus.Success;
        }

        Vector2 startPoint = tadpoles.leadTadpolePosition;
        Vector2 direction = tadpoles.aimingPosition - startPoint;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, LayerMask.GetMask("LookAtRegion"));
        if (hit.collider != null && hasStartWarning)
        {
            hasStartWarning = false;
            warningFade = Mathf.Clamp((Time.time - lastLookedAwayTime) / scaleTime, 0, 1);
            if (audio != null) audio.Stop();
        }
        else if (hit.collider == null && !hasStartWarning)
        {
            hasStartWarning = true;
            warningAnimationObject.Value.SetActive(true);
            lastLookedAwayTime = Time.time - warningFade;
            if (audio != null) audio.PlayAudio("punish_warning");
        }

        if(hasStartWarning)
        {
            float time = Time.time - lastLookedAwayTime;
            if (time >= allowLookedAwayTime.Value) 
            {
                Vector2 startPos = shootingBehavior.transform.position;
                Vector2 shootingDirection = tadpoles.leadTadpolePosition - startPos;
                shootingBehavior.setting.range = 50;
                shootingBehavior.Shoot(shootingDirection);
                
                hasStartWarning = false;
                warningAnimationObject.Value.SetActive(false);
                anim.animation.Play("idle");

                return TaskStatus.Success;
            }
            
            if(time < scaleTime)
            {
                warningAnim.animation.GotoAndPlayByProgress(warningAnimName, time / scaleTime);
            }
            else
            {
                warningAnim.animation.GotoAndPlayByProgress(warningAnimName, 1);
                if (Mathf.FloorToInt((time - scaleTime) / blinkInter) % 2 == 0)
                {
                    warningAnimationObject.Value.SetActive(false);
                }
                else
                {
                    warningAnimationObject.Value.SetActive(true);
                }
            }
        }else if(warningFade > 0)
        {
            Debug.Log(warningFade);
            warningFade = Mathf.MoveTowards(warningFade, 0, 10 * Time.deltaTime);
            warningAnim.animation.GotoAndPlayByProgress(warningAnimName, warningFade);
            if(warningFade <= 0.01)
            {
                warningFade = 0;
                warningAnimationObject.Value.SetActive(false);
            }
        }
        return TaskStatus.Running;
    }

}
