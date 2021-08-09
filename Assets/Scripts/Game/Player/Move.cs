using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    int walkspeed = 2;
    int runspeed = 5;
    int speed;

    void Update()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? runspeed : walkspeed;
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
}
