using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IMoveInput, IMouseAimInput, IShowPointInput
{
    #region Commands

    [SerializeField]
    private Command moveCommand;
    [SerializeField]
    private Command mouseAimCommand;
    [SerializeField]
    private Command shootCommand;
    [SerializeField]
    private Command defendCommand;

    #endregion

    private PlayerInputActionAsset playerInput;

    #region Interfaces

    public Vector2 moveDirection { get; private set; }

    public Vector2 mousePos { get; private set; }

    public bool usingMouse { get; private set; }

    public bool isShow { get; private set; }

    #endregion

    #region Callback Functions


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        moveDirection = value;
        if (moveCommand != null)
        {
            moveCommand.Execute();
        }
    }

    private void OnMouseAimInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        mousePos = value;
        usingMouse = true;
        if(mouseAimCommand != null)
        {
            mouseAimCommand.Execute();
        }
    }

    private void OnShootButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        if(shootCommand != null && value >= 0.15f)
        {
            shootCommand.Execute();
        }
    }
    
    private void OnDefendButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        if(defendCommand != null && value >= 0.15f)
        {
            defendCommand.Execute();
        }
    }

    private void OnShow(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        
        if(value < 0.1f)
        {
            isShow = false;
        }else if(value > 0.9f)
        {
            isShow = true;
        }

        
    }

    #endregion


    private void Awake()
    {
        playerInput = new PlayerInputActionAsset();
    }


    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Movement.performed += OnMoveInput;
        playerInput.Player.MouseAim.performed += OnMouseAimInput;
        playerInput.Player.Shoot.performed += OnShootButton;
        playerInput.Player.Defend.performed += OnDefendButton;
        playerInput.Player.ShowPoint.performed += OnShow;
    }


    private void OnDisable()
    {
        playerInput.Player.Movement.performed -= OnMoveInput;
        playerInput.Player.MouseAim.performed -= OnMouseAimInput;
        playerInput.Player.Shoot.performed -= OnShootButton;
        playerInput.Player.Defend.performed -= OnDefendButton;
        playerInput.Player.ShowPoint.performed -= OnShow;
        playerInput.Disable();
    }
}
