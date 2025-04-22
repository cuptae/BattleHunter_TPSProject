using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    //public Item item;
    public ParticleSystem twincle; // 아이템 파티클
    private ParticleSystem temp; // 파티클을 위해 복제 저장함
    public InventoryManager invenMgr; //아이템을 인벤토리에 넣기 위한 매니저
    public ItemClass itemClass; //아이템 정보가 있는 ScriptableObject를 받아옴
    private Rigidbody rb; // 아이템을 땡겨오기 위해 물리 적용
    private AudioClip itemaddsound; // 아이템 습득 효과음
    private bool canAdd = true; // 아이템이 연속으로 추가되지 않도록하는 변수
    private void Awake()
    {
        invenMgr = GameObject.Find("InventoryUI").GetComponent<InventoryManager>(); //인벤토리 매니저 찾기
        rb = GetComponent<Rigidbody>(); // 물리 컴포넌트 캐싱
        itemaddsound = Resources.Load("ItemAddSound") as AudioClip; // 효과음 리소스 불러오기
    }
    private void OnEnable() // 활성화 될 시
    {
        temp = Instantiate(twincle, this.transform.position, Quaternion.identity);
        temp.transform.SetParent(this.transform, true);
        temp.transform.localPosition = new Vector3(0, 0, 0);
        temp.transform.localScale = new Vector3(2, 2, 2);
        Vector3 randomDir = Random.insideUnitSphere;
        rb.AddForce(randomDir * 100f);
    }
    private void OnTriggerEnter(Collider other) // 플레이어가 아이템과 충돌할 때
    {
        Debug.Log("Enter: " + other.name);
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 아이템에 닿았어요!");
            InvenAdd();
            Destroy(this.gameObject);
        }
    }
    
    //인벤토리에 아이템 넣는 가상함수
    public void InvenAdd()
    {
        if (canAdd) // itemClass를 인벤토리에 1개만 추가시키도록함
            invenMgr.Add(itemClass, 1); 
        canAdd = false; // 중복 습득 방지
    }
}
