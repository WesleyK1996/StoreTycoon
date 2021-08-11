using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    NavMeshSurface[] NMS;

    void OnEnable()
    {
        NavMesh.RemoveAllNavMeshData();
        NMS = FindObjectsOfType<NavMeshSurface>();
        for (int i = 0; i < NMS.Length; i++)
        {
            NMS[i].BuildNavMesh();
        }
        enabled = false;
    }
}