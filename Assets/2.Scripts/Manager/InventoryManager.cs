using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;
    [SerializeField] public GameObject slotGhost;
    [SerializeField] private GameObject quickSlotGhost;
    [SerializeField] private RectTransform inventoryPanel;


    public SlotClass[] items;

    public GameObject[] slots;
    [SerializeField] private GameObject[] quickSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    [SerializeField] private GameObject sureBox;
    private SlotClass pendingDropSlot; // 임시 저장 슬롯
    private SlotClass pendingOriginalSlot; // 되돌릴 슬롯 위치
    [SerializeField] private GameObject trashSlot; // 휴지통 오브젝트 (UI 상에서 드래그할 수 있는 위치에 있어야 함)

    
    
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;

    public scJson jsondata;
    private void Start()
    {
        slots = new GameObject[slotGhost.transform.childCount];
        items = new SlotClass[slots.Length];
        quickSlots = new GameObject[quickSlotGhost.transform.childCount];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = quickSlotGhost.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        for (int i = 0; i < slotGhost.transform.childCount; i++)
        {
            slots[i] = slotGhost.transform.GetChild(i).gameObject;
        }
        RefreshUI();
        if (File.Exists(jsondata.path + jsondata.filename))
        {
            jsondata.Load();
        }
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        
        if (isMovingItem)
        {
            // 마우스 따라다니게
            itemCursor.transform.position = Input.mousePosition;
            // 드래그 중인 아이템 아이콘 표시
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
            itemCursor.GetComponent<Image>().enabled = true;
        }
        else
        {
            itemCursor.GetComponent<Image>().enabled = false;
        }

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
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                    Remove(GetClosestSlot().GetItem());
                }
                //무기아이템이면
                else if (GetClosestSlot().GetItem().GetTool())
                {
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                }
                //장비아이템이면
                else if (GetClosestSlot().GetItem().GetEquipment())
                {
                    GetClosestSlot().GetItem().Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
                }
                //기타잡템일때
                else if (GetClosestSlot().GetItem().GetMisc())
                {
                    
                }
            }
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

    public void UseSelected()
    {
        if (selectedItem == null)
            return;
        selectedItem.Use(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>());
        if (selectedItem.GetConsumable())
        {
            Remove(selectedItem);
        }
        else if(selectedItem.GetTool()||selectedItem.GetEquipment())
        {
            
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

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();

        // 휴지통 위로 드롭하면 삭제
    if (IsPointerOverTrashSlot())
    {
        Debug.Log("휴지통에 아이템을 버립니다: " + movingSlot.GetItem().itemName);
        sureBox.SetActive(true);
    }

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

    // 휴지통 슬롯에 마우스가 올라가 있는지 확인
private bool IsPointerOverTrashSlot()
{
    if (trashSlot == null) return false;
    return Vector2.Distance(trashSlot.transform.position, Input.mousePosition) <= 32f;
}

    public void DropItem()
{
    movingSlot.Clear();
    isMovingItem = false;
    if (pendingDropSlot != null && pendingDropSlot.GetItem() != null)
    {
        Debug.Log("아이템을 버렸습니다: " + pendingDropSlot.GetItem().itemName);
        pendingDropSlot.Clear(); // 삭제할 슬롯도 클리어
    }
    sureBox.SetActive(false);
    RefreshUI();
}

    public void CancelDropItem()
{
    movingSlot.Clear();
    isMovingItem = true;
    sureBox.SetActive(false);
    RefreshUI();
}



    #endregion Moving Stuff

    public SlotClass[] CurrentItems()
    {
        return this.items;
    }
}
