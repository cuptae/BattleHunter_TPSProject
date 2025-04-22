using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGun : MonoBehaviour
{
    public GameObject muzzleFlashPrefab;
    public Transform firePos;

    public float fireRate;
    public bool canFire = true;

    public AudioClip gunAudioClip; // 이름 수정: Clip은 오디오 데이터
    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 컴포넌트가 없으면 자동으로 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드
    }

public void Fire()
{
    Debug.Log("Fire!");

    // 1. 이펙트 생성
    Instantiate(muzzleFlashPrefab, firePos.position, firePos.rotation);

    // 2. 사운드 재생
    if (gunAudioClip != null)
    {
        audioSource.PlayOneShot(gunAudioClip);
    }

    // 3. Raycast로 충돌 감지 및 데미지 주기
    Ray ray = new Ray(firePos.position, firePos.forward);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 100f))
    {
        Debug.Log($"Hit: {hit.collider.name}");

        VRMantisHP target = hit.collider.GetComponent<VRMantisHP>();
        if (target != null)
        {
            target.TakeDamage(5); // 데미지 수치는 원하는 값으로 설정
            Debug.Log("Target hit! Damage dealt.");
        }
    }
}
}
