using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoCreate : MonoBehaviour
{
    public bool visible;
    public bool onlySelected;
    //기즈모 색상
    public Color myColor = Color.red;
    //기즈모 반지름
    public float radius = 0.05f;
    private void OnDrawGizmos() {
        if(!visible||onlySelected)
        {
            return;
        }
        Gizmos.color= myColor;
        Gizmos.DrawSphere(transform.position,radius);
    }
        void OnDrawGizmosSelected()
    {
        if(!visible||!onlySelected)
        {
            return;
        }
        Vector3 p = transform.position;
        Gizmos.color = myColor;
        Gizmos.DrawSphere(p, radius);
    }
}
