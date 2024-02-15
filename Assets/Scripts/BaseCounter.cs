using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform clearTopPoint;

    private KitchenObject kitchenObject;


    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter.Interact() method!");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter.InteractAlternate() method!");
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return clearTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
