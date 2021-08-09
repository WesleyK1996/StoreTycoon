using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform LeftHinge, RightHinge;

    private void OnTriggerEnter(Collider other)
    {
        float elapsed = 0f;
        // start = vector3 zero;
        while (elapsed < 1f)
        {
            LeftHinge.eulerAngles = Vector3.Lerp();
        }
    }
}
