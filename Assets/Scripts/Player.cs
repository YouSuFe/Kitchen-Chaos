using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCounter, IKitchenObjectParent
{
    public static Player Instance { get; private set;}

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private Vector3 lastInteractDirection;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There are more than one player on the field!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }


    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(movementVector.x, 0, movementVector.y);
        if(moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        float interactDistance = 2f;
        // Return a boolean value for if there is any gameobject in specific 
        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(baseCounter != selectedCounter)
                {
                    selectedCounter = baseCounter;

                    SetSelectedCounter(selectedCounter);
                } 
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(movementVector.x, 0, movementVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;
        float playerHeight = 2f;
        float playerRadius = 0.65f;
        Vector3 headPosition = transform.position + Vector3.up * playerHeight;
        Vector3 footPosition = transform.position;

        // Get a bool value if there is any obstacle on the way of the player
        bool canMove = !Physics.CapsuleCast(footPosition, headPosition, playerRadius, moveDirection, moveDistance);

        // Handle the diognal movement when the player faces an obstacle
        if (!canMove)
        {
            // Cannot move towards the moveDirection

            // Attempt only move on the x-direction
            Vector3 moveDirectionX = new Vector3(movementVector.x, 0, 0).normalized;
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(footPosition, headPosition, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                // Can move only on x-direction
                moveDirection = moveDirectionX;
                transform.position += moveDirection * moveDistance;
            }
            else
            {
                // Cannot move on the x-direction

                // Attempt only move on the z-direction
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = moveDirection.z != 0 && !Physics.CapsuleCast(footPosition, headPosition, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    // Can move only on z-direction
                    moveDirection = moveDirectionZ;
                    transform.position += moveDirection * moveDistance;
                }
                else
                {
                    return;
                }
            }
        }
        // Move freely if there is no obstacle
        else
        {
            transform.position += moveDirection * moveDistance;
        }

        // Check if the player has a speed and return a bool value true if it has
        isWalking = (moveDirection != Vector3.zero);

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);

    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
