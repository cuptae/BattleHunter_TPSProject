using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{

    [Header("Crafting Recipe")]
    public int recipeID;
    public SlotClass[] inputItems;
    public SlotClass outputItem;

    public bool CanCraft(InventoryManager inventory)
    {
        if (inventory.isFull())
        {
            return false;
        }
      
        for(int i = 0; i < inputItems.Length; i++)
        {
            if (!inventory.Contains(inputItems[i].GetItem(), inputItems[i].GetCount()))
            {
                return false;
            }
        }

        return true;
    }

    public void Craft(InventoryManager inventory)
    {
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].GetItem(), inputItems[i].GetCount());
        }
        inventory.Add(outputItem.GetItem(), outputItem.GetCount());
    }

   
}
