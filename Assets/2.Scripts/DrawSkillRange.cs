using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillRange : MonoBehaviour
{
    void Update()
    {
        for (int i = ActiveSkill.gizmoBoxes.Count - 1; i >= 0; i--)
        {
            ActiveSkill.gizmoBoxes[i].remainingTime -= Time.deltaTime;
            if (ActiveSkill.gizmoBoxes[i].remainingTime <= 0f)
                ActiveSkill.gizmoBoxes.RemoveAt(i);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f); // 초록, 반투명

        foreach (var box in ActiveSkill.gizmoBoxes)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(box.center, box.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawCube(Vector3.zero, box.size);
        }
    }
}


public class GizmoDrawRequest
{
    public Vector3 center;
    public Vector3 size;
    public Quaternion rotation;
    public float remainingTime;

    public GizmoDrawRequest(Vector3 center, Vector3 size, Quaternion rotation, float time)
    {
        this.center = center;
        this.size = size;
        this.rotation = rotation;
        this.remainingTime = time;
    }
}