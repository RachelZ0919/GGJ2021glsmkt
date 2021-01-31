using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Stats>().OnStatsChanged += CheckHP;
            GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
            boss.GetComponent<Stats>().OnStatsChanged += CheckHP;

        }
        else
        {
            Destroy(this);
        }
    }

    public void LevelFailed()
    {
        
    }


    public void LevelSucceed()
    {

    }

    private void CheckHP(Stats stat)
    {
        if (stat.CompareTag("Player"))
        {
            if (stat.health <= 0)
            {
                LevelFailed();
            }
        }
        else if (stat.CompareTag("Enemy"))
        {
            if (stat.health <= 0)
            {
                LevelSucceed();
            }
        }
    }

}
