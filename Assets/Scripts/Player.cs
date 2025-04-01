using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance{get; private set;}
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounterField;
    }
    
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private Vector3 lastMoveDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Error: Instance already exists!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }
    
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
        }

        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastMoveDirection, out RaycastHit hit, interactionDistance))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    selectedCounter = clearCounter;
                    SetSelectedCounter(selectedCounter);
                }
            }
            else if (selectedCounter)
            {
                selectedCounter = null;
                SetSelectedCounter(selectedCounter);
            }
        }
        else if (selectedCounter)
        {
            selectedCounter = null;
            SetSelectedCounter(selectedCounter);
        }

        
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
        
        // implement and visualize raycast
        // bool canMove = !Physics.Raycast(transform.position, movementDirection, playerSize);
        // Debug.DrawRay(transform.position, movementDirection * playerSize, Color.red);

        if (canMove)
        {
            //move to new position
            transform.position += moveDirection * moveDistance;
        }
        else
        {
            // Can't move to given position. Try to move horizontally
            //Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f);
            
            Vector3 moveDirectionX = Vector3.right * moveDirection.x;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                transform.position += moveDirectionX * moveDistance;
            }
            else
            {
                // Can't move horizontally. Try to move vertically
                //Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z);
                Vector3 moveDirectionZ = Vector3.forward * moveDirection.z;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    transform.position += moveDirectionZ * moveDistance;
                }
            }
        }
        
        isWalking = moveDirection != Vector3.zero;
        
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);    
    }
    
    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        this.selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounterField = selectedCounter
        });
    }
}
