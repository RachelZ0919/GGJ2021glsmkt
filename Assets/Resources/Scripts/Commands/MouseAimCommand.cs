using UnityEngine;
using System.Collections;


public class MouseAimCommand : Command
{
    [SerializeField]
    private Transform visualTransform;

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
        Vector2 rotateDirection = -tadpoles.aimingDirection;
        float angle = Vector2.SignedAngle(Vector2.left, rotateDirection);
        visualTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
