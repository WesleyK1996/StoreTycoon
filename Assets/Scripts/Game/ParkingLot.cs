using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkingSpace
{
    public bool free;
    public GameObject gameObject;
}

public class ParkingLot : MonoBehaviour
{
    public static ParkingLot Instance;
    public List<ParkingSpace> spaces = new List<ParkingSpace>();


    public delegate Vector3 DestinationMethod(Car car);
    public DestinationMethod GetDestination;
    Car car;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        switch (name)
        {
            case "Parking1":
                GetDestination = Parking1;
                break;
        }
    }

    Vector3 GetRoadCenterVector(string name, bool road)
    {
        foreach (Transform t in road ? transform.Find("Road") : transform.Find("Spaces"))
            if (t.name == name)
                return Car.GetRoadCenterVector(t.gameObject);
        return Vector3.zero;
    }

    Vector3 Parking1(Car car)//parking for lot 1
    {
        bool road = true;
        switch (car.target)
        {
            case "ParkingSpace1":
            case "ParkingSpace2":
            case "ParkingSpace3":
                road = false;
                car.SetStatus(Car.CarStatus.parked);
                break;
            case "A2":
                car.target = car.parkingSpace.gameObject.name;
                road = false;
                break;
            case "In":
                car.target = "A2";
                break;
        }

        return GetRoadCenterVector(car.target, road);
    }
}
