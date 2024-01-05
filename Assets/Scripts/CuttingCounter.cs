using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no KitchenObject
            if (player.HasKitchenObject())
            {
                // player has KitchenObject
                KitchenObject playerKitchenObject = player.GetKitchenObject();

                if (HasRecipeForInput(playerKitchenObject.GetKitchenObjectSO()))
                {
                    // recipe found
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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

            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(storedKitchenObject.GetKitchenObjectSO());

            if (outputKitchenObjectSO != null)
            {
                storedKitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeForInput(KitchenObjectSO input)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == input)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }

        return null;
    }
}