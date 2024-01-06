using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IProgressController
{
    public event EventHandler<IProgressController.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO appliedFryingRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (!HasKitchenObject())
        {
            return;
        }

        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                
                OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / appliedFryingRecipeSO.fryingTimerMax
                });
                
                if (fryingTimer > appliedFryingRecipeSO.fryingTimerMax)
                {
                    // Fried
                    fryingTimer = 0f;
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(appliedFryingRecipeSO.output, this);

                    state = State.Fried;
                    burningTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                }

                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                
                OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                {
                    progressNormalized = burningTimer / appliedFryingRecipeSO.burningTimerMax
                });
                
                if (burningTimer > appliedFryingRecipeSO.burningTimerMax)
                {
                    // Fried
                    burningTimer = 0f;
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(appliedFryingRecipeSO.burned, this);

                    state = State.Burned;
                    burningTimer = 0f;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    
                    OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }

                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no KitchenObject
            if (player.HasKitchenObject())
            {
                // player has KitchenObject
                KitchenObject playerKitchenObject = player.GetKitchenObject();

                FryingRecipeSO applicableRecipe = GetRecipeForInput(playerKitchenObject.GetKitchenObjectSO());

                if (applicableRecipe != null)
                {
                    // recipe found
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    appliedFryingRecipeSO = GetRecipeForInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    
                    OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / appliedFryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // player not carrying anything
            }
        }
        else
        {
            // there is KitchenObject
            if (player.HasKitchenObject())
            {
                // player has KitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // ingredient added to the plate
                        GetKitchenObject().DestroySelf();
                        
                        state = State.Idle;
                
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                
                        OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                
                OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private FryingRecipeSO GetRecipeForInput(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == input)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }
}