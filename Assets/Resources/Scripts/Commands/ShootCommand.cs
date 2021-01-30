using UnityEngine;
using System.Collections;


public class ShootCommand : Command
{
    private TadpoleGroup tadpoles;

    private void Awake()
    {
        tadpoles = GetComponentInChildren<TadpoleGroup>();
    }

    public override void Execute()
    {
        tadpoles.Shoot();
    }
}
