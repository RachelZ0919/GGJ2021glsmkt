using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleGroup : MonoBehaviour
{
    private Queue<TinyTadpole> availableTadpoles;
    private List<TinyTadpole> outTadpoles;
    private TadpoleFollowingPoints points;
    private GameObject defenceCircle;

    [HideInInspector]
    public Vector2 aimingDirection;
    public TinyTadpoleData tadpoleData;

    private float startDefendTime;
    private float lastDefendTime;
    private bool prepareToDefend;




    private void Awake()
    {
        availableTadpoles = new Queue<TinyTadpole>();
        outTadpoles = new List<TinyTadpole>();
        points = GetComponentInChildren<TadpoleFollowingPoints>();
        defenceCircle = transform.Find("DefenceCircle").gameObject;
        defenceCircle.SetActive(false);
    }

    private void Start()
    {
        Debug.Log("Start!");
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
    }

    private void Update()
    {
        if(Time.time - startDefendTime > tadpoleData.defendTime)
        {
            defenceCircle.SetActive(false);
        }
    }

    public bool Shoot()
    {
        if(availableTadpoles.Count != 0)
        {
            TinyTadpole tadpole = availableTadpoles.Dequeue();
            
            points.DeletePoint(tadpole.FollowTransform);
            tadpole.FollowTransform = null;
            
            tadpole.Shoot(aimingDirection);
            outTadpoles.Add(tadpole);
            return true;
        }

        return false;
    }

    public void CallbackTadpoles()
    {
        for (int i = 0; i < outTadpoles.Count; i++)
        {
            outTadpoles[i].GoBack();
        }
    }

    public void Comeback(TinyTadpole tadpole)
    {
        tadpole.transform.parent = transform;
        tadpole.FollowTransform = points.AddPoint();

        outTadpoles.Remove(tadpole);
        availableTadpoles.Enqueue(tadpole);

        if(prepareToDefend && outTadpoles.Count == 0)
        {
            prepareToDefend = false;
            Defend();
        }
    }

    public int GetTadpoleCount()
    {
        return availableTadpoles.Count;
    }


    public void ActivateDefence()
    {
        if(Time.time - lastDefendTime > tadpoleData.defendCooldown)
        {
            if(outTadpoles.Count == 0)
            {
                Defend();
            }
            else
            {
                prepareToDefend = true;
                CallbackTadpoles();
            }
        }
    }

    private void Defend()
    {
        for(int i = 0; i < availableTadpoles.Count; i++)
        {
            TinyTadpole tadpole = availableTadpoles.Dequeue();
            tadpole.Defend();
            availableTadpoles.Enqueue(tadpole);
        }
        defenceCircle.SetActive(true);
        points.MoveInCircle(tadpoleData.defendTime);
        startDefendTime = Time.time;
        lastDefendTime = Time.time + tadpoleData.defendTime;
    }

    public bool IsDefending()
    {
        return prepareToDefend || !points.returnedToPosition || Time.time - startDefendTime <= tadpoleData.defendTime;
    }

    public Transform GetAFollowPoint()
    {
        return points.AddPoint();
    }
}
