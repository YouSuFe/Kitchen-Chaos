using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cuttingKitchenObjectSo;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object here
            if (player.HasKitchenObject())
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
            if (player.HasKitchenObject())
            {
                // Player is carrying something
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            // There is a kitchen object
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cuttingKitchenObjectSo, this);
        }
    }

}
