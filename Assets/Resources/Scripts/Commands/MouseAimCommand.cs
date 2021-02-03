using UnityEngine;
using System.Collections;


public class MouseAimCommand : Command
{
    [SerializeField]
    private Transform visualTransform;
    [SerializeField]
    private Transform followingPointsTransform;

    private IMouseAimInput aimInput;
    private TadpoleGroup tadpoles;

    private Vector3 originScale;
    private Vector3 reverseScale;

    private void Awake()
    {
        aimInput = GetComponent<IMouseAimInput>();
        tadpoles = GetComponentInChildren<TadpoleGroup>();
        originScale = visualTransform.localScale;
        originScale.x = Mathf.Abs(originScale.x);
        reverseScale = new Vector3(-originScale.x, originScale.y, originScale.z);
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
        followingPointsTransform.rotation = Quaternion.Euler(0, 0, angle);

        angle = Vector2.SignedAngle(Vector2.right, direction);
        if (Mathf.Abs(angle) > 90)
        {
            visualTransform.localScale = reverseScale;
            visualTransform.rotation = Quaternion.Euler(0, 0, angle - 180);

        }
        else
        {
            visualTransform.localScale = originScale;
            visualTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
