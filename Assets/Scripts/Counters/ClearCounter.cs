using System;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object here
            if (player.HasKitchenObject())
            {
                // player is carrying a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // kitchen object here already
            if (player.HasKitchenObject())
            { 
                // player is carrying a kitchen object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyKitchenObject();
                    }
                }
                else
                {
                    // player is not holding a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter has a plate kitchen object
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroyKitchenObject();
                        }
                    }
                }
            }
            else
            {
                // player is not carrying anything
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
