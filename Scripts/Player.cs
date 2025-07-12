using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }//singleton pattern


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed= 7f;
    [SerializeField] private GameInput gameInput;

    private bool isWAlking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;


    private KitchenObject kitchenObejct;
    [SerializeField] private Transform kitchenObjectHoldPoint;



    private void Awake() //singleton pattern
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one Player Instance");
        }
        Instance = this;
    }


    private void Start() // on s'abonne dans le start car 
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()   
    {
        HandleMovement();
        HandleInteractions();

    }

    public bool IsWalking()
    {
        return isWAlking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir !=Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        //we are doing a raycats towards any obejct that has any kind of physics collider 
        //Raycast et raycastHit renvoient que le premier objet qu'ils touchent donc si y'avait un mur invisible 
        //on peut faire a la place soit un raycatall qui renvoie une array 
        //soit  ajouter un layerMask it will only hit objects within that layer 
        if ( Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if( raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(baseCounter !=selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
            //pour gerer le visuel on pourrait we can run some kind of logic on the selected counter and the counter would fire off some kind of events
            //saying it has been selected and the counter visual would listen to that    il dit que c'est bien if we want to add some kind of logic on
            //clearCOunter while selected 

            //Même chose mais plus compacte  pour le trygetcomponent 
            //ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
            //if (clearCounter != null)
            //{
            //}
        }
        else
        {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        //le bas et le haut de la capsule
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //attempt only x movement 
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can move only on the x axis 
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on the x axis

                //attempt only z movement 
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //can move only on the x axis 
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }


            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWAlking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        //le this.  représente le selectedCOunter de la classe 

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
            //le premier est le field dans OnSelectedCounterChangedEventArgs
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObejct = kitchenObject;
    }

    public KitchenObject GetKitchenObejct()
    {
        return kitchenObejct;
    }

    public void ClearKitchenObject()
    {
        kitchenObejct = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObejct != null;
    }
}
