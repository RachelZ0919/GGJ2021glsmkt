using UnityEngine;
using System.Collections;


public class MouseAimCommand : Command
{
    [SerializeField]
    private Transform visualTransform;

    private IMouseAimInput aimInput;
    private TadpoleGroup tadpoles;

    private Vector2 aimingPoint;

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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 mouseInput = new Vector3(aimInput.mousePos.x, aimInput.mousePos.y, screenPos.z);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseInput);
        Vector2 myPos = tadpoles.leadTadpolePosition;
        Vector2 direction = (mousePos - myPos).normalized;
        tadpoles.aimingPosition = mousePos;
        Vector2 rotateDirection = -direction;
        float angle = Vector2.SignedAngle(Vector2.left, rotateDirection);
        visualTransform.rotation = Quaternion.Euler(0, 0, angle);

        aimingPoint = mousePos;
    }
}
