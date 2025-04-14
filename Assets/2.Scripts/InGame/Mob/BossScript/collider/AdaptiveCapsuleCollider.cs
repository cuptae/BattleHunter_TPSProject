using UnityEngine;

[ExecuteAlways]
public class AdaptiveCapsuleCollider : MonoBehaviour
{
    [Header("뼈대 설정")]
    public Transform startBone;
    public Transform endBone;
    public Transform[] innerBones;

    [Header("반지름 오프셋 (여유 여백)")]
    public float extraRadius = 0.01f;

    [Header("디버그")]
    public bool showGizmos = true;

    private CapsuleCollider capsule;

    void Awake()
    {
        EnsureCapsuleExists();
    }

    void LateUpdate()
    {
        if (startBone == null || endBone == null || innerBones == null || innerBones.Length == 0)
            return;

        EnsureCapsuleExists();
        UpdateCapsuleTransformAndRadius();
    }

    void EnsureCapsuleExists()
    {
        if (capsule == null)
        {
            capsule = GetComponentInChildren<CapsuleCollider>();
            if (capsule == null)
            {
                GameObject capsuleGO = new GameObject("AutoCapsule");
                capsuleGO.transform.SetParent(this.transform);
                capsule = capsuleGO.AddComponent<CapsuleCollider>();
                capsule.isTrigger = true;
            }
        }
    }

    void UpdateCapsuleTransformAndRadius()
    {
        Vector3 a = startBone.position;
        Vector3 b = endBone.position;
        Vector3 dir = (b - a).normalized;
        float totalLength = Vector3.Distance(a, b);

        // 반지름 자동 계산
        float maxDistance = 0f;
        foreach (var bone in innerBones)
        {
            Vector3 projected = ProjectPointOnLine(a, b, bone.position);
            float dist = Vector3.Distance(bone.position, projected);
            if (dist > maxDistance)
                maxDistance = dist;
        }

        float finalRadius = maxDistance + extraRadius;

        // height = 전체 길이 (start ~ end) - 양쪽 구 반지름 2개
        float finalHeight = Mathf.Max(0.001f, totalLength - (finalRadius * 2f));

        // 캡슐 중심은 a~b의 정중앙
        Vector3 center = (a + b) / 2f;

        Transform t = capsule.transform;
        t.position = center;
        t.rotation = Quaternion.LookRotation(dir);
        capsule.direction = 2; // Z축
        capsule.radius = finalRadius;
        capsule.height = finalHeight + finalRadius * 2f; // Unity는 구 포함한 전체 높이를 요구
    }

    // 선분 a~b에 p를 수직 투영한 위치 반환
    Vector3 ProjectPointOnLine(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        float t = Mathf.Clamp01(Vector3.Dot(p - a, ab.normalized) / ab.magnitude);
        return a + ab * t;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos || startBone == null || endBone == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startBone.position, endBone.position);
        float r = capsule != null ? capsule.radius : 0.05f;
        Gizmos.DrawWireSphere(startBone.position, r);
        Gizmos.DrawWireSphere(endBone.position, r);
    }
}
