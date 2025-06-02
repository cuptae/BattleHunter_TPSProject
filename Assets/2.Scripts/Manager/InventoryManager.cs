using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;
    [SerializeField] public GameObject slotGhost;
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private GameObject trashCheckBox;

    public GameObject[] slots;   // public으로 변경 (외부 접근 가능)
    public SlotClass[] items;    // public으로 변경

    private SlotClass movingSlot;
    private SlotClass originalSlot;
    private SlotClass tempSlot;
    private SlotClass trashSlot;

    private int trashSlotIndex = -1;
    private bool isMovingItem;

    public scJson jsondata;
    public ItemClass selectedItem;

    private void Start()
    {
        slots = new GameObject[slotGhost.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotGhost.transform.GetChild(i).gameObject;
            items[i] = new SlotClass();
        }

        RefreshUI();

        if (File.Exists(jsondata.path + jsondata.filename))
            jsondata.Load();
    }

    private void Update()
    {
        if (!IngameUIManager.Instance.isOnPlaying)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
                EndItemMove();
            else
                BeginItemMove();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SlotClass targetSlot = GetClosestSlot();
            if (targetSlot != null && targetSlot.GetItem() != null)
            {
                ItemClass item = targetSlot.GetItem();
                PlayerCtrl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

                item.Use(player);
                if (item.GetConsumable())
                    Remove(item);
            }
        }

        UpdateCursorUI();
    }

    private void UpdateCursorUI()
    {
        itemCursor.SetActive(isMovingItem && movingSlot != null && movingSlot.GetItem() != null);
        if (itemCursor.activeSelf)
        {
            itemCursor.transform.position = Input.mousePosition;
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Image icon = slots[i].transform.GetChild(0).GetComponent<Image>();
            Text countText = slots[i].transform.GetChild(1).GetComponent<Text>();

            if (items[i].GetItem() != null)
            {
                icon.enabled = true;
                icon.sprite = items[i].GetItem().itemIcon;

                if (items[i].GetItem().isStackable)
                    countText.text = items[i].GetCount().ToString();
                else
                    countText.text = "";
            }
            else
            {
                icon.enabled = false;
                icon.sprite = null;
                countText.text = "";
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
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            slot.SubCount(1);
            if (slot.GetCount() <= 0)
                slot.Clear();

            RefreshUI();
            return true;
        }
        return false;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && item.isStackable)
                return items[i];
        }
        return null;
    }

    public bool Contains(ItemClass item, int count)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && items[i].GetCount() >= count)
                return true;
        }
        return false;
    }

    public bool isFull()
    {
        foreach (SlotClass slot in items)
        {
            if (slot.GetItem() == null)
                return false;
        }
        return true;
    }

    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
            return false;

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        SlotClass targetSlot = GetClosestSlot();

        // 인벤토리 영역 밖으로 드래그 시
        if (!IsPointerOverInventoryPanel())
        {
            trashCheckBox.SetActive(true);
            trashSlot = new SlotClass(movingSlot);
            trashSlotIndex = GetSlotIndex(originalSlot); 
            movingSlot.Clear();
            isMovingItem = false;
            RefreshUI();
            return true;
        }

        if (targetSlot == null)
        {
            // 빈 슬롯이 없으면 원래 자리로 되돌리기
            originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetCount());
        }
        else if (targetSlot.GetItem() == null)
        {
            targetSlot.AddItem(movingSlot.GetItem(), movingSlot.GetCount());
        }
        else if (targetSlot.GetItem() == movingSlot.GetItem() && movingSlot.GetItem().isStackable)
        {
            targetSlot.AddCount(movingSlot.GetCount());
        }
        else
        {
            tempSlot = new SlotClass(targetSlot);
            targetSlot.AddItem(movingSlot.GetItem(), movingSlot.GetCount());
            movingSlot = new SlotClass(tempSlot);
            RefreshUI();
            UpdateCursorUI();
            return true;
        }

        movingSlot.Clear();
        isMovingItem = false;
        RefreshUI();
        UpdateCursorUI();
        return true;
    }

    private bool IsPointerOverInventoryPanel()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, Input.mousePosition, null);
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
                return items[i];
        }
        return null;
    }

    private int GetSlotIndex(SlotClass slot)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == slot)
                return i;
        }
        return -1;
    }

    public void Sure()
    {
        trashSlot.Clear();
        trashCheckBox.SetActive(false);
        trashSlotIndex = -1;
        RefreshUI();
    }

    public void Not()
    {
        if (trashSlot != null && trashSlot.GetItem() != null && trashSlotIndex >= 0)
        {
            items[trashSlotIndex].AddItem(trashSlot.GetItem(), trashSlot.GetCount());
        }

        Debug.Log($"[Not()] trashSlot: {trashSlot.GetItem()?.itemName}, Count: {trashSlot.GetCount()}, Index: {trashSlotIndex}");


        trashSlot.Clear();
        trashSlotIndex = -1;
        isMovingItem = false;
        movingSlot.Clear();

        trashCheckBox.SetActive(false);
        RefreshUI();
    }

    public SlotClass[] CurrentItems()
    {
        return items;
    }
}
