using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IProgressController
{
    public event EventHandler<IProgressController.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no KitchenObject
            if (player.HasKitchenObject())
            {
                // player has KitchenObject
                KitchenObject playerKitchenObject = player.GetKitchenObject();

                CuttingRecipeSO applicableRecipe = GetRecipeForInput(playerKitchenObject.GetKitchenObjectSO());

                if (applicableRecipe != null)
                {
                    // recipe found
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    
                    OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / applicableRecipe.cuttingProgressMax
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
            }
            else
            {
                // player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // there is KitchenObject

            KitchenObject storedKitchenObject = GetKitchenObject();

            CuttingRecipeSO applicableRecipe = GetRecipeForInput(storedKitchenObject.GetKitchenObjectSO());

            if (applicableRecipe != null)
            {
                
                cuttingProgress++;
                
                OnCut?.Invoke(this, EventArgs.Empty);
                OnProgressChanged?.Invoke(this, new IProgressController.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / applicableRecipe.cuttingProgressMax
                });

                if (cuttingProgress == applicableRecipe.cuttingProgressMax)
                {
                    storedKitchenObject.DestroySelf();

                    KitchenObject.SpawnKitchenObject(applicableRecipe.output, this);

                    cuttingProgress = 0;
                }
            }
        }
    }

    private CuttingRecipeSO GetRecipeForInput(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == input)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}