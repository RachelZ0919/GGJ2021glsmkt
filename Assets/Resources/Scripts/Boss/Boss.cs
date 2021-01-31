using UnityEngine;
using DragonBones;


public class Boss : MonoBehaviour
{
    private UnityArmatureComponent armatureComponent;

    private void Awake()
    {
        armatureComponent = transform.Find("anim").GetComponent<UnityArmatureComponent>();
        GetComponent<Stats>().OnStatsChanged += CheckDeath;
    }

    private void CheckDeath(Stats stat)
    {
        if(stat.health <= 0)
        {
            stat.OnStatsChanged -= CheckDeath;
            armatureComponent.AddDBEventListener(EventObject.LOOP_COMPLETE, OnDeathAnimationEnd);
            armatureComponent.animation.Play("die");
        }
    }
    private void OnDeathAnimationEnd(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name == "die")
        {
            armatureComponent.animation.GotoAndStopByProgress("die", 1);
            GameManager.instance.LevelSucceed();
            armatureComponent.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnDeathAnimationEnd);

        }
    }
}
