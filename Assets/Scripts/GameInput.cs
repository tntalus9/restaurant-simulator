using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += InteractOnPerformed;
    }

    private void InteractOnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        // Vector2 inputVector = new Vector2(0, 0);
        // if (Input.GetKey(KeyCode.W))
        // {
        //     inputVector.y = 1;
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     inputVector.x = -1;
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     inputVector.y = -1;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     inputVector.x = 1;
        // }
        
        inputVector = inputVector.normalized;
        return inputVector;
    }
    
}
