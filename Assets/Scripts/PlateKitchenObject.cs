using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validIngredients;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validIngredients.Contains(kitchenObjectSO))
        {
            return false;
        }
        
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // already on the plate
            return false;
        }

        kitchenObjectSOList.Add(kitchenObjectSO);
        return true;
    }
}