using UnityEngine;
using System.Collections;


public class MouseAimCommand : Command
{
    [SerializeField]
    private Transform visualTransform;

    private IMouseAimInput aimInput;
    private TadpoleGroup tadpoles;

    private void Awake()
    {
        aimInput = GetComponent<IMouseAimInput>();
        tadpoles = GetComponentInChildren<TadpoleGroup>();
    }

    public override void Execute()
    {

    }

    public void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(aimInput.mousePos);
        Vector2 myPos = tadpoles.transform.position;
        tadpoles.aimingDirection = (mousePos - myPos).normalized;
        Vector2 rotateDirection = -tadpoles.aimingDirection;
        float angle = Vector2.SignedAngle(Vector2.left, rotateDirection);
        visualTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
