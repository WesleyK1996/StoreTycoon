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

    bool parked;
    bool coming = true;
    string road;

    //-2.49272
    //17.12534

    private void Start()
    {
        road = Random.Range(0, 2) == 0 ? "Top1" : "Bottom1";
        if (road == "Top1")
            transform.eulerAngles += new Vector3(0, 180, 0);
        transform.position = GetRoadCenterVector(road);

        agent.enabled = true;
        agent.SetDestination(GetDestination());
    }

    void Update()
    {
        if (!parked)
            Drive();
    }

    void Drive()
    {
        foreach (Transform wheel in wheels)
            wheel.Rotate(-Vector3.forward * rb.velocity.z);
        if (agent.remainingDistance < .5f && !parked)
            agent.SetDestination(GetDestination());
    }

    private Vector3 GetDestination()
    {
        if (coming)
        {
            switch (road)
            {
                case "Top1":
                    road = "Top4";
                    break;
                case "Bottom1":
                    road = "Bottom5";
                    break;
                case "Top4":
                case "Bottom5":
                    road = "Top5";
                    break;
                case "Top5":
                    road = "In";
                    break;
                case "In":
                    return GetParkingSpace();
                    break;
            }
        }
        else
        {

        }
        print(road);
        return GetRoadCenterVector(road);
    }

    private Vector3 GetParkingSpace()
    {
        throw new System.NotImplementedException();
    }

    Vector3 GetRoadCenterVector(string road)
    {
        Vector3 r = Vector3.zero;

        foreach (Transform t in GameManager.Instance.road)
            if (t.name == road)
            {
                r = t.GetComponent<Collider>().bounds.center;
                if (road.Contains("Bottom"))
                    r.z -= .4f;
                r.y = 1;
                return r;
            }
        return r;
    }
}
