using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    [SerializeField] private GameObject itemCursor;
    [SerializeField] public GameObject slotHolder;
    [SerializeField] private GameObject quickSlotHolder;
    //[SerializeField] private ItemClass itemToAdd;
    //[SerializeField] private ItemClass itemToRemove;
    //[SerializeField] private SlotClass[] startingItems;

    public SlotClass[] items;

    public GameObject[] slots;
    [SerializeField] private GameObject[] quickSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    [SerializeField] private GameObject quickSlotSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;

    private AudioClip eatSound;
    private AudioClip equipChangeSound;

    public scJson jaondata;
    private void Awake()
    {
        eatSound = Resources.Load("eatSound") as AudioClip;
        equipChangeSound = Resources.Load("equipChangeSound") as AudioClip;
    }
    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        quickSlots = new GameObject[quickSlotHolder.transform.childCount];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        //for (int i = 0; i < startingItems.Length; i++)
        //{
        //    items[i] = startingItems[i];
        //}
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
        //Add(itemToAdd, 1);
        //Remove(itemToRemove);
        if (File.Exists(jaondata.path + jaondata.filename))
        {
            jaondata.Load();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Craft(craftingRecipes[0]);
        }
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;

        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(GetClosestSlot().GetItem());
            if (isMovingItem)
            {
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }

        // if (isMovingItem)
        // {
        //     // 마우스 따라다니게
        //     itemCursor.transform.position = Input.mousePosition;

        //     // 드래그 중인 아이템 아이콘 표시
        //     itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        //     itemCursor.GetComponent<Image>().enabled = true;
        // }
        // else
        // {
        //     itemCursor.GetComponent<Image>().enabled = false;
        // }

        // 클릭으로 아이템 이동 시작/종료 처리
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
                EndItemMove();
            else
                BeginItemMove();
        }
        
        else if (Input.GetMouseButtonDown(1))

        {
            if (GetClosestSlot() != null)
            {
                //소모품이면
                if (GetClosestSlot().GetItem().GetConsumable())
                {
                    LobbySoundManager.Instance.PlayEffect(eatSound);
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                    Remove(GetClosestSlot().GetItem());
                }
                //무기아이템이면
                else if (GetClosestSlot().GetItem().GetTool())
                {
                    LobbySoundManager.Instance.PlayEffect(equipChangeSound);
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                }
                //장비아이템이면
                else if (GetClosestSlot().GetItem().GetEquipment())
                {
                    LobbySoundManager.Instance.PlayEffect(equipChangeSound);
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                }
                //기타잡템일때
                else if (GetClosestSlot().GetItem().GetMisc())
                {
                    if (isMovingItem)
                    {
                        EndItemMove_Single();
                    }
                    else
                    {
                        BeginItemMove_Half();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 0, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 1, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 2, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 3, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 4, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 5, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 6, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex = 7, 0, quickSlots.Length);
            selectedItem = items[selectedSlotIndex].GetItem();
            UseSelected();
        }

        quickSlotSelector.transform.position = quickSlots[selectedSlotIndex].transform.position;
    }

    private void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.CanCraft(this))
        {
            recipe.Craft(this);
        }
        else
        {
            Debug.Log("Can't crafting Item!");
        }
    }

    #region Inventory Utils
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetCount() + "";
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }

        }

        RefreshQuickSlot();
    }

    public void RefreshQuickSlot()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            try
            {
                quickSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                quickSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                {
                    quickSlots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetCount() + "";
                }
                else
                {
                    quickSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
                }
            }
            catch
            {
                quickSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                quickSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                quickSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }

        }
    }
    public bool Add(ItemClass item, int count)
    {
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            slot.AddCount(count);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, count);
                    break;
                }
            }
            //if (items.Count < slots.Length)
            //{
            //    items.Add(new SlotClass(item, 1));
            //}
            //else
            //{
            //    return false;
            //}
        }
        RefreshUI();
        return true;
    }
    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetCount() >= 1)
            {
                temp.SubCount(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }
        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item, int count)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetCount() > 1)
            {
                temp.SubCount(count);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }
        RefreshUI();
        return true;
    }

    public void UseSelected()
    {
        //items[selectedSlotIndex].SubCount(1);
        //RefreshUI();
        if (selectedItem == null)
            return;
        selectedItem.Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
        if (selectedItem.GetConsumable())
        {
            LobbySoundManager.Instance.PlayEffect(eatSound);
            Remove(selectedItem);
        }
        else if(selectedItem.GetTool()||selectedItem.GetEquipment())
        {
            LobbySoundManager.Instance.PlayEffect(equipChangeSound);
        }
        RefreshUI();
    }

    public bool isFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == null)
            {
                return false;
            }
        }
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && items[i].GetItem().isStackable)
            {
                return items[i];
            }
        }
        return null;
    }

    public bool Contains(ItemClass item, int count)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //Debug.Log(items[i]);
            if (items[i].GetItem() == item && items[i].GetCount() >= count)
            {
                return true;
            }
        }
        return false;
    }
    #endregion Inventory Utils

    #region Moving Stuff
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetCount() / 2f));
        originalSlot.SubCount(Mathf.CeilToInt(originalSlot.GetCount() / 2f));

        if (originalSlot.GetCount() == 0)
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetCount());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddCount(movingSlot.GetCount());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetCount());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetCount());
                    RefreshUI();
                    return true;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetCount());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return false;
        }
        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
        {
            return false;
        }
        movingSlot.SubCount(1);
        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddCount(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
        }
        if (movingSlot.GetCount() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }
        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i];
            }
        }
        return null;
    }
    #endregion Moving Stuff

    public SlotClass[] CurrentItems()
    {
        return this.items;
    }
}