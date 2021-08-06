using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    public StoreManager manager;

    enum FieldLetter
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H
    }
    FieldLetter fieldLetter;

    public enum SquareLetter
    {
        A,
        B,
        C,
        D,
        E
    }
    SquareLetter squareLetter;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(FindObjectOfType<StoreManager>().gameObject);
        Destroy(GameObject.FindGameObjectWithTag("Parking"));
        manager = Instantiate(Resources.Load(Path.Combine("Prefabs","Store")) as GameObject).GetComponent<StoreManager>();
        //storeManager.transform.position = 
        manager.store.name = "New Store";
        manager.store.money = 2000;
        manager.store.parkingLot = manager.build.SpawnParkingLot(1);

        for (int i = 0; i < 64; i++)
            manager.store.fields.Add(MakeField(i));
        MakeStartStore();
    }

    Store.Field MakeField(int i)
    {
        Store.Field field = new Store.Field();
        field.name = FieldName(i);
        field.field = GetField(field.name);
        field.squares = GetSquares();

        return field;
    }

    private GameObject GetField(string fieldname)
    {
        foreach(Transform Row in manager.transform)
        {
            if(Row.name == fieldname[0].ToString())
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

    private List<Store.Field.Square> GetSquares()
    {
        List<Store.Field.Square> r = new List<Store.Field.Square>();
        for (int i = 0; i < 25; i++)
        {
            r.Add(new Store.Field.Square() { name = SquareName(i) });
        }
        return r;
    }

    private string FieldName(int i)
    {
        fieldLetter = (FieldLetter)Mathf.FloorToInt(i / 8f);
        return fieldLetter.ToString() + (i % 8 + 1);
    }

    private string SquareName(int i)
    {
        squareLetter = (SquareLetter)Mathf.FloorToInt(i / 5f);
        return squareLetter.ToString() + (i % 5 +1);
    }

    private void MakeStartStore()
    {
        MakeFloors();
        MakeWalls();
    }

    private void MakeFloors()
    {
        foreach (Transform row in manager.transform)
        {
            foreach (Transform column in row)
            {
                if (row.name + column.name == "A4" || row.name + column.name == "A5")
                {
                    for (int i = 0; i < 25; i++)
                        manager.build.MakeFloor(column, SquareName(i), "Concrete");
                }
            }
        }
    }

    private void MakeWalls()
    {

    }
}
