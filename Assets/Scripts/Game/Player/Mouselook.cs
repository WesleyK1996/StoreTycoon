using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouselook : MonoBehaviour
{
    float sensitivity;
    float xRot;
    float yRot;
    Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        sensitivity = 300f;//replace with playerprefs
        xRot = -90;
        yRot = 0;
        playerBody = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            xRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            yRot += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            yRot = ClampAngle(yRot, -90, 90);
            transform.localRotation = Quaternion.AngleAxis(yRot, -Vector3.right);
            playerBody.rotation = Quaternion.AngleAxis(xRot, Vector3.up);
        }
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
