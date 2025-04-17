using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildRecipe", menuName = "Building/Recipe")]
public class BuildRecipeClass : ScriptableObject
{


    [Header("Building Recipe")]
    public Sprite buildImage;
    public string buildName;

    public SlotClass[] inputItems;

    public bool needWorkbench;
    // Start is called before the first frame update
    public bool CanBuild(InventoryManager inventory)
    {

        for (int i = 0; i < inputItems.Length; i++)
        {
            if (!inventory.Contains(inputItems[i].GetItem(), inputItems[i].GetCount()))
            {
                return false;
            }
        }

        return true;
    }

    public void Build(InventoryManager inventory)
    {
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].GetItem(), inputItems[i].GetCount());
        }
        //
    }
}
