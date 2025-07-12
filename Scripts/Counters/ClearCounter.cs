using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO; 



    public override void Interact(Player player)
    { 
        if(!HasKitchenObject()) // there is no KitchenObject there
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObejct().SetKitchenObjectParent(this);
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
            }
        }
       
    }

    
}
