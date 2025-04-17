using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButtonItemInfo : MonoBehaviour
{
    public CraftingRecipeClass recipe;
    public InventoryManager inven;
    public CraftManager craftManager;
    
    public int buttonId;
    
    public Button btn;
    public Button craftBtn;
    public ItemClass item;
    public GameObject[] requestSlots;
    public GameObject itemImage;
    public GameObject itemName;
    public GameObject InfoItemImage;
    public GameObject ItenInfoName;
    public GameObject ItemInfoDesc;
    public GameObject requestItemImage1;
    public GameObject requestItemCount1;
    public GameObject requestItemImage2;
    public GameObject requestItemCount2;

    public Color color;
    public bool temp = true;

    public bool isActive = false;

    private void Awake()
    {
        itemImage.GetComponent<Image>().sprite = item.GetItem().itemIcon;
        itemName.GetComponent<Text>().text = item.GetItem().itemName;
    }

    public void Start()
    {
        btn.onClick.AddListener(OnClick);

        
        
    }

   

    public void OnClick()
    {

        SendButtonId(buttonId);
        Debug.Log(buttonId);


        InfoItemImage.GetComponent<Image>().sprite = item.GetItem().itemIcon;
        ItenInfoName.GetComponent<Text>().text = item.GetItem().itemName;
        ItemInfoDesc.GetComponent<Text>().text = item.GetItem().itemDesc;
        
        for (int i = 0; i < requestSlots.Length; i++)
        {
            try
            {
                Debug.Log("try");
                ReturnColor(i);

                craftBtn.interactable = true;
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = recipe.inputItems[i].GetItem().itemIcon;
                requestSlots[i].transform.GetChild(1).GetComponent<Text>().text = recipe.inputItems[i].GetCount() + "";    
            }
            catch
            {
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                requestSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            if (!recipe.CanCraft(inven))
            {
                craftBtn.interactable = false;
                if (!inven.Contains(recipe.inputItems[i].GetItem(), recipe.inputItems[i].GetCount()))
                {
                    ChangeColor(i);
                }
            }
            

        }
        //if (recipe.inputItems[0] != null && recipe.inputItems[1] != null)
        //{
        //    requestItemImage1.GetComponent<Image>().sprite = recipe.inputItems[0].GetItem().itemIcon;
        //    requestItemCount1.GetComponent<Text>().text = recipe.inputItems[0].GetCount().ToString();
        //    requestItemImage2.GetComponent<Image>().sprite = recipe.inputItems[1].GetItem().itemIcon;
        //    requestItemCount2.GetComponent<Text>().text = recipe.inputItems[1].GetCount().ToString();
        //}
        //else
        //{
        //    requestItemImage2.GetComponent<Image>().sprite = null;
        //    requestItemCount2.GetComponent<Text>().text = null;
        //}

    }

    public int SendButtonId (int _buttonId)
    {
       return craftManager.btnIndex = _buttonId;
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

    void ChangeColor(int i)
    {
        
        requestSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1, 0, 0, 1);
    }

    void ReturnColor(int i)
    {
        
        requestSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1, 1, 1, 1);
    }
    

    
}
