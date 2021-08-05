using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject SpawnParkingLot(int version)
    {
        switch (version)
        {
            case 2:
                break;
        }
        return Instantiate(Resources.Load(Path.Combine("Prefabs", "Parking", "Parking" + version)) as GameObject);
    }

    public void MakeFloor(Transform field, string square, string floor)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Floors", floor)) as GameObject, field);
        go.transform.position = GetFloorPos(square);
    }

    private Vector3 GetFloorPos(string square)
    {
        int x;
        for (int i = 0; i < 5; i++)
        {
            if()
        }
        return new Vector3(x, 0, -int.Parse(square[1].ToString()));
    }
}
