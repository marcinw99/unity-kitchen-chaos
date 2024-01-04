using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no KitchenObject
            if (player.HasKitchenObject())
            {
                // player has KitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
}