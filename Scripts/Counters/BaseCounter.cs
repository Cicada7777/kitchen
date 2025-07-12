using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObejct;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.Interact");
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
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
