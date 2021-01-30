using UnityEngine;
using System.Collections;


public class ShowCommand : Command
{
    private IShowPointInput showPointInput;

    private void Awake()
    {
        showPointInput = GetComponent<IShowPointInput>();
    }

    public override void Execute()
    {
        //todo:显示判定点
        if (showPointInput.isShow)
        {

        }
        else
        {

        }
    }

}
