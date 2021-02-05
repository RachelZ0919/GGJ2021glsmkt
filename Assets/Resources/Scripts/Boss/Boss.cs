using UnityEngine;
using DragonBones;


public class Boss : MonoBehaviour
{
    private UnityArmatureComponent armatureComponent;
    [SerializeField] private AudioManager audio;
    [SerializeField] private AudioClip deathSoundEffect;

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
            
            if (audio == null)
            {
                Debug.LogError("Need audio manager to play boss death sound effect!");
            }
            else if (deathSoundEffect == null)
            {
                Debug.LogError("There is no boss death sound effect!");
            }
            else
            {
                audio.PlayAudio(deathSoundEffect);
            }
        }
    }
    private void OnDeathAnimationEnd(string type, EventObject eventObject)
    {
        if (eventObject.animationState.name == "die")
        {
            armatureComponent.animation.GotoAndStopByProgress("die", 1);
            armatureComponent.RemoveDBEventListener(EventObject.LOOP_COMPLETE, OnDeathAnimationEnd);
            armatureComponent.gameObject.SetActive(false);
            GameManager.instance.LevelSucceed();
        }
    }
}
