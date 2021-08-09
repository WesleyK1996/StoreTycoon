using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform leftHinge, rightHinge;

    private void OnTriggerEnter(Collider other)
    {
        print("Open");
        StartCoroutine(LerpDoor(true));
    }

    private void OnTriggerExit(Collider other)
    {
        print("Close");
        StartCoroutine(LerpDoor(false));
    }

    IEnumerator LerpDoor(bool open)
    {
        Vector3 start, left, right;
        start = Vector3.zero;
        left = new Vector3(0, 90, 0);
        right = new Vector3(0, -90, 0);
        float elapsed = 0f;
        // start = vector3 zero;
        while (elapsed < 1f)
        {
            if (open)
            {
                leftHinge.localEulerAngles = Vector3.Lerp(start, left, elapsed);
                rightHinge.localEulerAngles = Vector3.Lerp(start, right, elapsed);
            }
            else
            {
                leftHinge.localEulerAngles = Vector3.Lerp(left, start, elapsed);
                rightHinge.localEulerAngles = Vector3.Lerp(right, start, elapsed);
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
