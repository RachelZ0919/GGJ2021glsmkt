using UnityEngine;
using System.Collections;


public class MouseAimCommand : Command
{
    private IMouseAimInput aimInput;
    private Rigidbody2D rigidbody;
    private TadpoleGroup tadpoles;

    private void Awake()
    {
        aimInput = GetComponent<IMouseAimInput>();
        tadpoles = GetComponentInChildren<TadpoleGroup>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void Execute()
    {

    }

    public void Update()
    {
        //todo:更新射击瞄准位置,待debug
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(aimInput.mousePos);
        tadpoles.aimingDirection = (mousePos - rigidbody.position).normalized;

    }
}
