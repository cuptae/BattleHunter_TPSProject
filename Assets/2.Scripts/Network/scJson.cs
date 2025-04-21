using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class scJson : MonoBehaviour
{
    public string path;
    public string filename;
    public PlayerCtrl thePlayer;
    public InventoryManager invenMgr;
    public ItemClass[] itemList;
    
    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerCtrl>();
        path = Application.dataPath+"/Resources/";
        filename = "save";
    }

    private void Start()
    {

    }

    public void SetPlayer(PlayerCtrl player)
    {
        thePlayer = player;
    }
        
    public void Save()
    {
        JsonData jsondata = new JsonData(); 
        jsondata.items = new List<InventoryItem>(); 
        jsondata.playerPos = thePlayer.transform.position; 
        jsondata.playerRot = thePlayer.transform.rotation; 
        
        //여기에 들어갈 로직 - 인벤토리의 모든 슬롯의 개수만큼 반복
        for (int i = 0; i < invenMgr.slots.Length; i++)
        {
            //여기에 들어갈 로직 - invenMgr.items[i]!=null
            if (invenMgr.items[i].GetItem() != null)
            {
                InventoryItem invenItem = new InventoryItem();
                invenItem.itemID = invenMgr.items[i].GetItem().itemID;
                invenItem.itemAmount = invenMgr.items[i].GetCount();
                jsondata.items.Add(invenItem); 
                Debug.Log(jsondata.items[i].itemID);
                Debug.Log(jsondata.items[i].itemAmount);
            }
        }
        //플레이어 포지션 각도
        //인벤토리 검사로직을 써서 인벤토리 내부에있는 아이템 이름 ,수량 ,인덱스를
        //jsondata.invenitem <<LIST안에 하나씩 저장 -> 제이슨파일로 변환 파일저장
        
        string json = JsonUtility.ToJson(jsondata);
        File.WriteAllText(path + filename, json);
        Debug.Log(2);
    }

    public void Load()
    {
        Debug.Log("Load");        
        string json = File.ReadAllText(path + filename);
        JsonData jsondata = JsonUtility.FromJson<JsonData>(json);
        Debug.Log(jsondata.items[0].itemID);
        Debug.Log(jsondata.items.Count);
        thePlayer.transform.position = jsondata.playerPos;
        thePlayer.transform.rotation = jsondata.playerRot;
        Debug.Log(invenMgr.slots.Length);

        //인벤토리를 비움
        for (int i = 0; i < invenMgr.slots.Length; i++)
        {
            Debug.Log(invenMgr.slots.Length);
            if (invenMgr.items[i].GetItem() != null)
            {
                invenMgr.items[i].Clear();
                invenMgr.RefreshUI();
            }
        }

        for(int i = 0; i < jsondata.items.Count; i++)
        {
            InventoryItem item = jsondata.items[i];

            for(int j = 0; j < itemList.Length; j++)
            {
                ItemClass iteminfo = itemList[j];
                if(iteminfo.itemID == item.itemID)
                {
                    invenMgr.Add(iteminfo, item.itemAmount);
                }
            }
        }
        //제이슨으로 가져온 정보를 인벤토리에 저장
    }
}

[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    public int itemID;
    [SerializeField]
    public int itemAmount;

    public int ID // 아이템 ID (제품코드)
    {    
        get { return itemID; }        
        set { itemID = value; }
    }

    public int Count // 아이템 갯수
    {
        get { return itemAmount; }
        set { itemAmount = value; }
    }
}

[System.Serializable]
public class JsonData
{
    public List<InventoryItem> items;
    public Vector3 playerPos;
    public Quaternion playerRot;
}
