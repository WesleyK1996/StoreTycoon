using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    Store store;

    void Start()
    {
        try
        {
            Destroy(FindObjectOfType<StoreManager>().gameObject);
        }
        catch { }

        store = Instantiate(Resources.Load(Path.Combine("Prefabs", "Store")) as GameObject).GetComponent<StoreManager>().store;
        store.name = "New Store";
        store.money = 2000;
        Build.BuildParkingLot(1);

        for (int i = 0; i < 64; i++)
            store.fields.Add(Build.IntToFieldString(i), MakeField(i));
        MakeStartStore();

    }

    Store.Field MakeField(int n)
    {
        Store.Field field = new Store.Field();
        field.name = Build.IntToFieldString(n);
        field.field = Build.GetField(field.name).gameObject;
        for (int i = 0; i < 25; i++)
            field.squares.Add(Build.IntToSquareString(i), MakeSquare(field.field.transform, i));

        return field;
    }

    private Store.Field.Square MakeSquare(Transform parent, int n)
    {
        Store.Field.Square square = new Store.Field.Square() { self = new GameObject() };
        square.self.transform.parent = parent;
        square.self.name = Build.IntToSquareString(n);
        square.self.transform.localPosition = Build.SquareToPos(square.self.name);
        return square;
    }

    private void MakeStartStore()
    {
        MakeFloors();
        MakeWalls();
        MakeCeilings();

        StartCoroutine(JsonStuff.SaveJsonFile(store, true));
    }

    private void MakeFloors()
    {
        for (int i = 0; i < 25; i++)
            Build.BuildFloor("ConcreteFloor", "A4", Build.IntToSquareString(i));

        for (int i = 0; i < 25; i++)
            Build.BuildFloor("ConcreteFloor", "A5", Build.IntToSquareString(i));
    }

    private void MakeWalls()
    {
        Build.BuildWall("ConcreteWall", "A4", "A5", Build.SquarePositions.Xm);
        Build.BuildWall("ConcreteDoor", "A4", "A3", Build.SquarePositions.Xm);
        Build.BuildWall("ConcreteWall", "A4", "A1", Build.SquarePositions.Xm);
        Build.BuildWall("ConcreteWall", "A4", "A1", Build.SquarePositions.Zp);
        Build.BuildWall("ConcreteWall", "A4", "B1", Build.SquarePositions.Zp);
        Build.BuildWall("ConcreteWall", "A4", "C1", Build.SquarePositions.Zp);
        Build.BuildWall("ConcreteWall", "A4", "D1", Build.SquarePositions.Zp);
        Build.BuildWall("ConcreteWall", "A4", "E1", Build.SquarePositions.Zp);
        Build.BuildWall("ConcreteWall", "A4", "E1", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A4", "E2", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A4", "E3", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A4", "E4", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A4", "E5", Build.SquarePositions.Xp);

        Build.BuildWall("ConcreteWall", "A5", "E1", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A5", "E2", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A5", "E3", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A5", "E4", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A5", "E5", Build.SquarePositions.Xp);
        Build.BuildWall("ConcreteWall", "A5", "E5", Build.SquarePositions.Zm);
        Build.BuildWall("ConcreteWall", "A5", "D5", Build.SquarePositions.Zm);
        Build.BuildWall("ConcreteWall", "A5", "C5", Build.SquarePositions.Zm);
        Build.BuildWall("ConcreteWall", "A5", "B5", Build.SquarePositions.Zm);
        Build.BuildWall("ConcreteWall", "A5", "A5", Build.SquarePositions.Zm);
        Build.BuildWall("ConcreteWall", "A5", "A5", Build.SquarePositions.Xm);
        Build.BuildWall("ConcreteDoor", "A5", "A3", Build.SquarePositions.Xm);
        Build.BuildWall("ConcreteWall", "A5", "A1", Build.SquarePositions.Xm);
    }

    private void MakeCeilings()
    {
        Transform field = Build.GetField("A4");
        string type;
        foreach (Transform square in field)
        {
            type = square.name == "C3" ? "ConcreteLight" : "ConcreteCeiling";
            Build.BuildCeiling(type, "A4", square.name);
        }

        field = Build.GetField("A5");
        foreach (Transform square in field)
        {
            type = square.name == "C3" ? "ConcreteLight" : "ConcreteCeiling";
            Build.BuildCeiling(type, "A5", square.name);
        }
    }
}
