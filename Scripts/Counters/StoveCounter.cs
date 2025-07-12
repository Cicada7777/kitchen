using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnPorgressChangedEventArgs> OnProgressChanged;

    public event EventHandler< OnStateChangedEventsArs> OnStateChanged;
        public class OnStateChangedEventsArs: EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle, 
        Frying,
        Fried,
        Burned,
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float burningTimer;
    private float fryingTimer;

    private BurningRecipeSO burningRecipeSO;
    private FryingRecipeSO fryingRecipeSO;


    private void Start()
    {
        state= State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnPorgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObejct().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state= State.Fried;
                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSoWithInput(GetKitchenObejct().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventsArs
                        {
                            state = state
                        });

                       

                    }
                    break;
                case State.Fried:

                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnPorgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObejct().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventsArs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Burned:

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnPorgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                    break;
            }
        }
        
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // there is no KitchenObject there
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObejct().GetKitchenObjectSO()))
                {
                    player.GetKitchenObejct().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObejct().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventsArs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnPorgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                }
            }
            else
            {

            }
        }
        else //there is a KitchenObject
        {
            if (player.HasKitchenObject())
            {

            }
            else
            {
                GetKitchenObejct().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventsArs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnPorgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

}
