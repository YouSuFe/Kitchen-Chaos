using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Put a kitchen object on the clear counter
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            // There is no kitchen object here
            if(player.HasKitchenObject())
            {
                // Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            // There is a kitchen object
            if(player.HasKitchenObject())
            {
                // Player is carrying something
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }

}
