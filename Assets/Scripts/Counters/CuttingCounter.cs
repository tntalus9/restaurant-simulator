using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArtgs> OnProgressChanged;
    public class OnProgressChangedEventArtgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object here
            if (player.HasKitchenObject())
            {
                // player is carrying a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                { 
                    // player is carrying an object that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArtgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
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
        if (HasKitchenObject() && HasRecipeWithInput(this.GetKitchenObject().GetKitchenObjectSO()))
        {
            // There's a kitchen object here AND it can be cut
            cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArtgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress == cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObectSO = GetOutputForInput(this.GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroyKitchenObject();
                KitchenObject.SpawnKitchenObject(outputKitchenObectSO,this); 
            }
            
            
        }
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSOForInput(inputKitchenObjectSO);
        return cuttingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
