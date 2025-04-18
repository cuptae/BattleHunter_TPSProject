using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scCanBuild : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildRecipeClass recipe;
    public InventoryManager inven;
    
    public BuildManager buildManager;
    public Button btn;
    public int btnId;
    public GameObject[] requestSlots;

    public GameObject InfoItemImage;
    public GameObject ItenInfoName;

    public GameObject requestItemImage1;
    public GameObject requestItemCount1;
    public GameObject requestItemImage2;
    public GameObject requestItemCount2;
    // Start is called before the first frame update
  
    public void OnPointerEnter(PointerEventData eventDate)
    {

        InfoItemImage.GetComponent<Image>().enabled = true;
        InfoItemImage.GetComponent<Image>().sprite = recipe.buildImage;
        ItenInfoName.GetComponent<Text>().text = recipe.buildName;
        

        for (int i = 0; i < requestSlots.Length; i++)
        {
            ReturnColor(i);
            try
            {
                ReturnColor(i);
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = recipe.inputItems[i].GetItem().itemIcon;
                requestSlots[i].transform.GetChild(1).GetComponent<Text>().text = recipe.inputItems[i].GetCount() + "";
                btn.interactable = true;
            }
            catch
            {
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                requestSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                requestSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            

            if (!recipe.CanBuild(inven))
            {
                if (!inven.Contains(recipe.inputItems[i].GetItem(), recipe.inputItems[i].GetCount()))
                {
                    ChangeColor(i);


                }
            }
        }
            //btn.interactable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoItemImage.GetComponent<Image>().sprite = null;
        InfoItemImage.GetComponent<Image>().enabled = false;
        ItenInfoName.GetComponent<Text>().text = null;

        for (int i = 0; i < requestSlots.Length; i++)
        {
            requestSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            requestSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            requestSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
        }
    }

    void ChangeColor(int i)
    {
        btn.interactable = false;
        requestSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1, 0, 0, 1);
    }

    void ReturnColor(int i)
    {

        requestSlots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1, 1, 1, 1);
    }
}
