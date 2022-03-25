using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtility : MonoBehaviour
{
    public static Vector3 RaycastPosition(Vector3 origin, Vector3 direction)
    {
        var ray = new Ray(origin, direction);
        if (Physics.SphereCast(ray, .25F, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                Debug.Log($"hitting {hit.collider.name}");
                return hit.point;
            }
        }
        
        return direction;
    }
}
