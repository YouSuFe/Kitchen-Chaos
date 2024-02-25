using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;

        waitingRecipeList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(waitingRecipeList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeList[Random.Range(0, recipeListSO.recipeList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeList.Add(waitingRecipeSO);
            }
            
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipeList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;

                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the Recipe

                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the Plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredients matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if(!ingredientFound)
                    {
                        // This Recipe ingredient was not fonud on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    Debug.Log("Player delivered the correct recipe!");
                    waitingRecipeList.RemoveAt(i);
                    return;
                }
            }
        }
        // No matches found
        // Player did not devliver a correct recipe

        Debug.Log("Player did not delivered the correct recipe!");
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeList;
    }
}
