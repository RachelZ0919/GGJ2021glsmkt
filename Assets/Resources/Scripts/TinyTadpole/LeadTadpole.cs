using UnityEngine;
using DragonBones;


public class LeadTadpole : MonoBehaviour
{
    [SerializeField]
    private UnityArmatureComponent armatureComponent;
    private Animator animator;

    private void Awake()
    {
        GetComponent<Stats>().OnStatsChanged += CheckDeath;
        animator = GetComponent<Animator>();
        armatureComponent.AddDBEventListener(EventObject.LOOP_COMPLETE, OnDeathAnimationEnd);
    }

    private void CheckDeath(Stats stat)
    {
        if(stat.health <= 0)
        {
            animator.SetBool("isDead", true);
            armatureComponent.animation.Play("GGJ-lead-2(die)");
            stat.OnStatsChanged -= CheckDeath;
        }
    }

    private void OnDeathAnimationEnd(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name == "GGJ-lead-2(die)")
        {
            armatureComponent.animation.GotoAndStopByProgress("GGJ-lead-2(die)", 1);
            GameManager.instance.LevelFailed();
            armatureComponent.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnDeathAnimationEnd);

        }
    }

}
