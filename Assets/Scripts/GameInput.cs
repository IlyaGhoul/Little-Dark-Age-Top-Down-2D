using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public PlayerInputAction _playerInputActions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputAction();
        _playerInputActions.Enable();
            
        _playerInputActions.Player.Attack.started += PlayerAttack_started;
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

}
