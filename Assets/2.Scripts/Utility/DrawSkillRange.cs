using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillRange : MonoBehaviour
{
    void Update()
    {
        for (int i = ActiveSkill.gizmo.Count - 1; i >= 0; i--)
        {
            var box = ActiveSkill.gizmo[i];
            box.remainingTime -= Time.deltaTime;
            if (box.remainingTime <= 0f)
                ActiveSkill.gizmo.RemoveAt(i);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f); // 초록, 반투명

        foreach (var gizmo in ActiveSkill.gizmo)
        {
            switch (gizmo.drawType)
            {
                case GizmoDrawRequest.DrawType.Box:
                    Matrix4x4 matrix = Matrix4x4.TRS(gizmo.center, gizmo.rotation, Vector3.one);
                    Gizmos.matrix = matrix;
                    Gizmos.DrawCube(Vector3.zero, gizmo.size);
                    break;

                case GizmoDrawRequest.DrawType.Sphere:
                    Gizmos.matrix = Matrix4x4.identity;
                    Gizmos.DrawSphere(gizmo.center, gizmo.size.x * 0.5f); // size.x는 지름
                    break;
            }
        }
    }
}



public class GizmoDrawRequest
{
    public enum DrawType { Box, Sphere }

    public Vector3 center;
    public Vector3 size; // Box는 size, Sphere는 지름으로 사용
    public Quaternion rotation;
    public float remainingTime;
    public DrawType drawType;

    public GizmoDrawRequest(Vector3 center, Vector3 size, Quaternion rotation, float duration, DrawType type)
    {
        this.center = center;
        this.size = size;
        this.rotation = rotation;
        this.remainingTime = duration; // <- 자동으로 넣어줌
        this.drawType = type;
    }
}

