using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private GameInput gameInput;


    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
    }

    private void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector3 normalizedInputVector = gameInput.GetMovementVectorNormalized();

        Vector3 positionChange = new Vector3(normalizedInputVector.x, 0f, normalizedInputVector.y);

        if (positionChange != Vector3.zero)
        {
            lastInteractDir = positionChange;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
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
        Vector3 normalizedInputVector = gameInput.GetMovementVectorNormalized();

        Vector3 positionChange = new Vector3(normalizedInputVector.x, 0f, normalizedInputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, positionChange, moveDistance);

        if (!canMove)
        {
            // attempt X only or Y only movements
            Vector3 positionChangeX = new Vector3(positionChange.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, positionChangeX, moveDistance);

            if (canMove)
            {
                positionChange = positionChangeX;
            }
            else
            {
                Vector3 positionChangeZ = new Vector3(0, 0, positionChange.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, positionChangeZ, moveDistance);

                if (canMove)
                {
                    positionChange = positionChangeZ;
                }
            }
        }
        

        if (canMove)
        {
            transform.position += positionChange * moveDistance;
        }
        
        isWalking = positionChange != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, positionChange, Time.deltaTime * rotateSpeed);
    }

    public void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs()
        {
            selectedCounter = selectedCounter
        });
    }
}
