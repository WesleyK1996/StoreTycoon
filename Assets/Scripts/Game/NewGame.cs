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
        try
        {
            Destroy(FindObjectOfType<StoreManager>().gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Parking"));
        }
        catch { }
        manager = Instantiate(Resources.Load(Path.Combine("Prefabs", "Store")) as GameObject).GetComponent<StoreManager>();
        manager.store.name = "New Store";
        manager.store.money = 2000;
        manager.build.BuildItem(null, Build.BuildableItemType.Parking, "Parking1", "");
        manager.store.parkingLot = GameObject.FindGameObjectWithTag("Parking");

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

    private List<Store.Field.Square> GetSquares()
    {
        List<Store.Field.Square> r = new List<Store.Field.Square>();
        for (int i = 0; i < 25; i++)
        {
            GameObject go = new GameObject();

           
            r.Add(new Store.Field.Square() { square = Instantiate(go) });
            go.name = SquareName(i);
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
        return squareLetter.ToString() + (i % 5 + 1);
    }

    private void MakeStartStore()
    {
        MakeFloors();
        MakeWalls();
    }

    private void MakeFloors()
    {
        Transform field = GetField("A4").transform;
        for (int i = 0; i < 25; i++)
            manager.build.BuildItem(field, Build.BuildableItemType.Floors, "Concrete", SquareName(i));
        field = GetField("A5").transform;
        for (int i = 0; i < 25; i++)
            manager.build.BuildItem(field, Build.BuildableItemType.Floors, "Concrete", SquareName(i));
    }

    private void MakeWalls()
    {
        Transform field = GetField("A4").transform;
        manager.build.BuildItem(field, Build.BuildableItemType.Walls, "Concrete", "A1",Build.SquarePositions.Xneg);
    }
}
