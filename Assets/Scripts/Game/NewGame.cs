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

        StartCoroutine(JsonStuff.SaveJsonFile(JsonStuff.StoreToStoreData(store), true));
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
        Build.BuildWall(Build.GetSquare(field, "A5"), "Concrete", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "Concrete", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "Concrete", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "B1"), "Concrete", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "C1"), "Concrete", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "D1"), "Concrete", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "E1"), "Concrete", Build.SquarePositions.Zpos);
        Build.BuildWall(Build.GetSquare(field, "E1"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E2"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E3"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E4"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "Concrete", Build.SquarePositions.Xpos);
        field = Build.GetField("A5").transform;
        Build.BuildWall(Build.GetSquare(field, "E1"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E2"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E3"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E4"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "Concrete", Build.SquarePositions.Xpos);
        Build.BuildWall(Build.GetSquare(field, "E5"), "Concrete", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "D5"), "Concrete", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "C5"), "Concrete", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "B5"), "Concrete", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "A5"), "Concrete", Build.SquarePositions.Zneg);
        Build.BuildWall(Build.GetSquare(field, "A5"), "Concrete", Build.SquarePositions.Xneg);
        Build.BuildWall(Build.GetSquare(field, "A1"), "Concrete", Build.SquarePositions.Xneg);
    }
}
