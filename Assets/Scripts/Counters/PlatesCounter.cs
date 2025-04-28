using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // player is not carrying a kitchen object
            if (platesSpawnedAmount > 0)
            {
                if (platesSpawnedAmount == platesSpawnedAmountMax)
                {
                    spawnPlateTimer = 0f;
                }
                platesSpawnedAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            }
        }
    }
    
    
}
