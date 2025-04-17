using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    private AudioClip craftingSound;

    public CraftButtonItemInfo buttonInfo;
    public CraftingRecipeClass[] recipe;
    //public CraftButtonItemInfo Axe;
    //public CraftButtonItemInfo Sword;
    //public CraftButtonItemInfo Bow;
    //public CraftButtonItemInfo Chest;
    //public CraftButtonItemInfo Leg;

    public InventoryManager inven;


    public int btnIndex;

    //public void Craft(CraftingRecipeClass recipe)
    //{

    //    if (recipe.CanCraft(inven))
    //    {
    //        recipe.Craft(inven);
    //    }

    //}

    private void Awake()
    {
        craftingSound = Resources.Load("craftingSound") as AudioClip;
    }
    public void Start()
    {


    }

    public void Update()
    {
       
        //if (buttonInfo[0].)
        //{
        //    btnIndex = buttonInfo[0].SendButtonId(buttonInfo[0].buttonId);
        //}
        //else if (buttonInfo[1].isActive)
        //{
        //    btnIndex = buttonInfo[1].SendButtonId(buttonInfo[1].buttonId);
        //}
    }


    public void OnClick()
    {
        LobbySoundManager.Instance.PlayEffect(craftingSound);
        switch (btnIndex)
        {
            case 1:
                Craft(recipe[0]);
                break;
            case 2:
                Craft(recipe[1]);
                break;
            case 3:
                Craft(recipe[2]);
                break;
            case 4:
                Craft(recipe[3]);
                break;
            case 5:
                Craft(recipe[4]);
                break;
            case 6:
                Craft(recipe[5]);
                break;
            case 7:
                Craft(recipe[6]);
                break;
        }
       
    }

    

    public void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.CanCraft(inven))
        {
            recipe.Craft(inven);
        }
        else
        {
            Debug.Log("Can't crafting Item!");
        }
    }
}
