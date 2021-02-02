using UnityEngine;
using System.Collections;


public class ShowCommand : Command
{
    private IShowPointInput showPointInput;
    [SerializeField]
    private GameObject point;


   
    private void Awake()
    {
        showPointInput = GetComponent<IShowPointInput>();
    }

    public override void Execute()
    {
        //todo:显示判定点
        if (showPointInput.isShow)
        {
            if (point != null) point.SetActive(true);
        }
        else
        {
            if (point != null) point.SetActive(false);
        }
    }

}
