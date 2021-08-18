using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    public Transform[] wheels;
    public Rigidbody rb;
    public NavMeshAgent agent;

    public string target;
    Vector3 targetPos;
    bool driving;
    bool inLot;

    public ParkingSpace parkingSpace;


    public delegate Vector3 DestinationMethod();
    DestinationMethod GetDestination;

    public enum CarStatus
    {
        coming,
        parking,
        parked,
        going
    }
    CarStatus status = CarStatus.coming;

    private void Start()
    {
        target = Random.Range(0, 2) == 0 ? "Top1" : "Bottom1";
        if (target == "Top1")
            transform.eulerAngles += new Vector3(0, 180, 0);
        transform.position = GetRoadCenterVector();

        agent.enabled = true;
    }

    void Update()
    {
        if (!driving && status != CarStatus.parked)
            StartCoroutine(Drive());
        if (driving)
            foreach (Transform wheel in wheels)
                wheel.Rotate(Vector3.forward, -1f);
    }

    IEnumerator Drive()
    {
        driving = true;

        SetBehaviour();
        targetPos = GetDestination();
        if (status == CarStatus.parked)
        {
            SpawnCustomers();
            driving = false;
            yield break;
        }
        agent.SetDestination(targetPos);

        while (Vector3.Distance(transform.position, targetPos) > agent.stoppingDistance)
            yield return new WaitForEndOfFrame();
        driving = false;
    }

    public void SpawnCustomers()
    {
        transform.eulerAngles = parkingSpace.gameObject.transform.eulerAngles;

        agent.enabled = false;
        enabled = false;
    }

    private void SetBehaviour()
    {
        switch (status)
        {
            case CarStatus.coming:
                GetDestination = GetDestinationComing;
                break;
            case CarStatus.parking:
                GetDestination = GetDestinationParking;
                break;
            case CarStatus.going:
                GetDestination = GetDestinationGoing;
                break;
        }
    }

    Vector3 GetDestinationComing()
    {
        switch (target)
        {
            case "Top1":
                target = "Top4";
                break;
            case "Bottom1":
                target = "Bottom5";
                break;
            case "Top4":
            case "Bottom5":
                if (CanPark(out parkingSpace))
                {
                    status = CarStatus.parking;
                    target = "Top5";
                }
                else
                    target = target == "Top4" ? "Top10" : "Bottom10";
                break;
            case "Top10":
            case "Bottom10":
                Destroy(agent);
                Destroy(gameObject);
                break;
        }
        return GetRoadCenterVector();
    }

    Vector3 GetDestinationParking()
    {
        switch (target)
        {
            case "Top5":
                target = "In";
                return GetRoadCenterVector();
            default:
                return ParkingLot.Instance.GetDestination(this);
        }
    }

    Vector3 GetDestinationGoing()
    {
        throw new System.NotImplementedException();
    }

    bool CanPark(out ParkingSpace parkingSpace)
    {
        foreach (ParkingSpace space in FindObjectOfType<ParkingLot>().spaces)
            if (space.free)
            {
                parkingSpace = space;
                space.free = false;
                return true;
            }
        parkingSpace = null;
        return false;
    }

    Vector3 GetRoadCenterVector()
    {
        foreach (Transform t in GameManager.Instance.road)
            if (t.name == target)
                return GetRoadCenterVector(t.gameObject);
        return Vector3.zero;
    }

    public static Vector3 GetRoadCenterVector(GameObject road)
    {
        Vector3 r = road.GetComponent<Collider>().bounds.center;
        r.y += 1;
        if (road.name.StartsWith("Bottom")) r.z -= .4f;

        return r;
    }

    public void SetStatus(CarStatus status)
    {
        this.status = status;
    }
}
