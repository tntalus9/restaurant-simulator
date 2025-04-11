using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO; 
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
            }
            else
            {
                // player is not carrying anything
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void AlternateInteract(Player player)
    {
        if (HasKitchenObject())
        {
            // There's a kitchen object here
            GetKitchenObject().DestroyKitchenObject();
            Transform kitchenObjectTransform = Instantiate(cutKitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
    }
}
