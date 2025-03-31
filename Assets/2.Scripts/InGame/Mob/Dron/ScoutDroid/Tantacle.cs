using UnityEngine;
using System.Collections.Generic;

public class Tentacle : MonoBehaviour
{
    public Transform rootTarget;         // 뿌리(시작점)가 따라갈 대상 (드론의 특정 위치)
    public float followSpeed = 10f;
    public float smoothness = 0.1f;
    public float spacing = 0.5f;
    public float waveAmp = 0.2f;
    public float waveFreq = 3f;

    private List<Transform> joints = new List<Transform>();
    private Vector3[] positions;
    private Vector3[] velocities;

    void Awake()
    {
        // 하위 마디 자동 수집
        joints.Clear();
        Transform current = transform;
        while (current.childCount > 0)
        {
            current = current.GetChild(0);
            joints.Add(current);
        }

        int len = joints.Count;
        positions = new Vector3[len];
        velocities = new Vector3[len];

        for (int i = 0; i < len; i++)
            positions[i] = joints[i].position;
    }

    void Update()
    {
        if (joints.Count == 0 || rootTarget == null) return;

        // Root 따라감
        positions[0] = Vector3.SmoothDamp(positions[0], rootTarget.position, ref velocities[0], 1f / followSpeed);

        for (int i = 1; i < joints.Count; i++)
        {
            Vector3 dir = (positions[i] - positions[i - 1]).normalized;
            Vector3 desiredPos = positions[i - 1] - dir * spacing;

            float wave = Mathf.Sin(Time.time * waveFreq + i * 0.5f) * waveAmp;
            Vector3 offset = Vector3.up * wave;

            positions[i] = Vector3.SmoothDamp(positions[i], desiredPos + offset, ref velocities[i], smoothness);
        }

        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].position = positions[i];
            if (i > 0)
                joints[i - 1].LookAt(joints[i]);
        }
    }
}
