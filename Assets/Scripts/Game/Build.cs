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
        go.transform.localPosition = SquareToPos(square);
        for (int i = 0; i < StoreManager.Instance.store.fields.Count; i++)
        {
            if (StoreManager.Instance.store.fields[i].field = field.gameObject)
            { }
        }
    }

    private Vector3 SquareToPos(string square)
    {
        print(square);
        int x = 0;
        switch (square[0])
        {
            case 'A':
                x = 0;
                break;
            case 'B':
                x = 1;
                break;
            case 'C':
                x = 2;
                break;
            case 'D':
                x = 3;
                break;
            case 'E':
                x = 4;
                break;
        }
        return new Vector3(x, -.25f, -int.Parse(square[1].ToString()) + 1);
    }
}
