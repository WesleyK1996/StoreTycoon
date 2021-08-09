using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    Store store = new Store();
    void Start()
    {
        try
        {
            Destroy(FindObjectOfType<StoreManager>().gameObject);
        }
        catch { }
        Instantiate(Resources.Load(Path.Combine("Prefabs", "Store")) as GameObject);

        store.name = "New Store";
        store.money = 2000;
        store.parkingLot = Build.BuildParkingLot(1);

        for (int i = 0; i < 64; i++)
            store.fields.Add(MakeField(i));

        MakeStartStore();

        FindObjectOfType<StoreManager>().store = store;
    }

    Store.Field MakeField(int n)
    {
        Store.Field field = new Store.Field();
        field.name = Build.IntToFieldString(n);
        field.field = Build.GetField(field.name).gameObject;
        for (int i = 0; i < 25; i++)
            field.squares.Add(MakeSquare(field.field.transform, i));

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

        //StartCoroutine(JsonStuff.SaveJsonFile(JsonStuff.StoreToStoreData(store), true));
    }

    private void MakeFloors()
    {
        int fieldN = Build.StringToFieldInt("A4");
        Transform field = store.fields[fieldN].field.transform;
        for (int i = 0; i < 25; i++)
            store.fields[fieldN].squares[i].floor = Build.BuildFloor(Build.GetSquare(field, Build.IntToSquareString(i)), "Concrete");

        fieldN = Build.StringToFieldInt("A5");
        field = store.fields[fieldN].field.transform;
        for (int i = 0; i < 25; i++)
            store.fields[fieldN].squares[i].floor = Build.BuildFloor(Build.GetSquare(field, Build.IntToSquareString(i)), "Concrete");
    }

    private void MakeWalls()
    {
        Transform field = Build.GetField("A4").transform;
        Build.BuildWall(Build.GetSquare(field, "A5"), "ConcreteWall", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A3"), "ConcreteDoor", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "ConcreteWall", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "ConcreteWall", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "B1"), "ConcreteWall", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "C1"), "ConcreteWall", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "D1"), "ConcreteWall", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "E1"), "ConcreteWall", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "E1"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E2"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E3"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E4"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "ConcreteWall", Build.SquarePositions.Xpos);

        field = Build.GetField("A5").transform;
        Build.BuildWall(Build.GetSquare(field, "E1"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E2"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E3"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E4"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "ConcreteWall", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "ConcreteWall", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "D5"), "ConcreteWall", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "C5"), "ConcreteWall", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "B5"), "ConcreteWall", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "A5"), "ConcreteWall", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "A5"), "ConcreteWall", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A3"), "ConcreteDoor", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "ConcreteWall", Build.SquarePositions.Xneg);
    }
    private void MakeCeilings()
    {
        Transform field = Build.GetField("A4");
        string type;
        foreach(Transform square in field)
        {
            type = square.name == "C3" ? "ConcreteWindow" : "ConcreteCeiling";
            Build.BuildCeiling(square, type)
        }

        field = Build.GetField("A5");

    }
}
