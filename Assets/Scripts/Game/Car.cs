using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    [Tooltip("First 2 values need to be the front wheels. otherwise the car won't steer correctly.")]
    public Transform[] wheels;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public Transform target;

    bool left;
    bool parked;

    private void Start()
    {
        left = Random.Range(0, 2) == 0 ? true : false;
    }

    void Update()
    {
        if (!parked)
            Drive();
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(-Vector3.forward * rb.velocity.z);
        }
    }

    void Drive()
    {
        if (target != null && agent.destination != target.position)
            agent.SetDestination(target.position);
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            agent.SetDestination(GetDestination());
    }

    private Vector3 GetDestination()
    {
        return Vector3.zero;
    }
}
