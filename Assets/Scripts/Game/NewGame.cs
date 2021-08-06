using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    public StoreManager manager;

    void Start()
    {
        print(0);
        try
        {
            Destroy(FindObjectOfType<StoreManager>().gameObject);
        }
        catch { }
        manager = Instantiate(Resources.Load(Path.Combine("Prefabs", "Store")) as GameObject).GetComponent<StoreManager>();
        manager.store.name = "New Store";
        manager.store.money = 2000;
        manager.build.SpawnParkingLot(1);
        manager.store.parkingLot = GameObject.FindGameObjectWithTag("Parking");

        //print(1);
        //for (int i = 0; i < 64; i++)
        //    manager.store.fields.Add(MakeField(i));
        //MakeStartStore();
    }

    Store.Field MakeField(int i)
    {
        print("making field " + (i + 1));
        Store.Field field = new Store.Field();
        field.name = Build.FieldName(i);
        field.field = GetField(field.name);
        field.squares = GetSquares(field.field.transform);

        return field;
    }

    private GameObject GetField(string fieldname)
    {
        foreach (Transform Row in manager.transform)
        {
            if (Row.name == fieldname[0].ToString())
            {
                foreach (Transform Column in Row)
                {
                    if (Column.name == fieldname[1].ToString())
                        return Column.gameObject;
                }
            }
        }
        return null;
    }

    private List<Store.Field.Square> GetSquares(Transform parent)
    {
        List<Store.Field.Square> r = new List<Store.Field.Square>();
        for (int i = 0; i < 25; i++)
        {
            GameObject go = null;
            r.Add(new Store.Field.Square() { square = go = new GameObject() });
            go.transform.parent = parent;
            go.name = Build.SquareName(i);
            go.transform.localPosition = Build.SquareToPos(Build.SquareName(i));
        }
        return r;
    }

    private void MakeStartStore()
    {
        MakeFloors();
        MakeWalls();
    }

    private void MakeFloors()
    {
        print("making floors");
        Transform field = GetField("A4").transform;
        for (int i = 0; i < 25; i++)
            manager.build.BuildFloor(field, Build.SquareName(i), "Concrete");

        field = GetField("A5").transform;
        for (int i = 0; i < 25; i++)
            manager.build.BuildFloor(field, Build.SquareName(i), "Concrete");
    }

    private void MakeWalls()
    {
        Transform field = GetField("A4").transform;
        manager.build.BuildWall(field, "A1", "Concrete", Build.SquarePositions.Xneg);
    }
}
