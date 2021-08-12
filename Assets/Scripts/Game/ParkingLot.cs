using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkingSpace
{
    public bool free;
    public GameObject space;
}

public class ParkingLot : MonoBehaviour
{
    public List<ParkingSpace> spaces = new List<ParkingSpace>();
}
