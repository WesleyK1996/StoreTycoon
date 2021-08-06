using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Build : MonoBehaviour
{
    public enum BuildableItemType
    {
        Parking,
        Floors,
        Walls,
        Ceilings
    }

    public enum SquarePositions
    {
        Xpos,
        Xneg,
        Zpos,
        Zneg
    }

    public void BuildItem(Transform field, BuildableItemType type, string itemName, string square)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", type.ToString(), itemName)) as GameObject, field);
        go.name = go.name.Replace("(Clone)", "");
        switch (type)
        {
            case BuildableItemType.Parking:
                SpawnParkingLot(go);
                break;
            case BuildableItemType.Floors:
                MakeFloor(field.gameObject, go, SquareToPos(square));
                break;
        }
    }

    public void BuildItem(Transform field, BuildableItemType type, string itemName, string square, SquarePositions side)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", type.ToString(), itemName)) as GameObject, field);
        go.name = go.name.Replace("(Clone)", "");
        switch (type)
        {
            case BuildableItemType.Walls:
                BuildWall(field.gameObject, go, side, SquareToPos(square));
                break;
        }
    }

    public void SpawnParkingLot(GameObject go)
    {
        try
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Parking"))
                if (obj != go)
                    Destroy(obj);
        }
        catch { }
        switch (go.name.Replace("Parking", ""))
        {
            case "1":
                go.transform.position = new Vector3(16.66694f, -1.605933f, -3.630101f);
                StoreManager.Instance.transform.position = new Vector3(43, 0, -5);
                break;
            default: throw new NotImplementedException();
        }
        StoreManager.Instance.store.parkingLot = go;
    }

    private void MakeFloor(GameObject field, GameObject floor, Vector3 pos)
    {
        floor.transform.localPosition = pos;
        for (int i = 0; i < StoreManager.Instance.store.fields.Count; i++)
            if (StoreManager.Instance.store.fields[i].field == field.gameObject)
                for (int j = 0; j < StoreManager.Instance.store.fields[i].squares.Count; j++)
                    StoreManager.Instance.store.fields[i].squares[j].floor = floor;
    }

    private Vector3 SquareToPos(string square)
    {
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

    public void BuildWall(GameObject field, GameObject wall, SquarePositions side, Vector3 pos)
    {
        Vector3 adjustment = Vector3.zero;
        switch(side)
        {
            case SquarePositions.Xpos:

                break;
            case SquarePositions.Xneg:
                break;
            case SquarePositions.Zpos:
                break;
            case SquarePositions.Zneg:
                break;
        }
        
    }
}
