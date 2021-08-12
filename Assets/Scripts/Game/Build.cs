using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Build : MonoBehaviour
{
    static StoreManager manager;
    public static bool loading;
    public enum FieldLetter
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

    public enum SquarePositions
    {
        Xp,
        Xm,
        Zp,
        Zm
    }

    private void OnEnable()
    {
        manager = GetComponent<StoreManager>();
    }

    public static void BuildParkingLot(int type)
    {
        Destroy(GameObject.FindGameObjectWithTag("Parking"));
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Parking", "Parking" + type)) as GameObject);

        go.name = go.name.Replace("(Clone)", "");
        switch (go.name.Replace("Parking", ""))
        {
            case "1":
                go.transform.position = new Vector3(16.66694f, -1.605933f, -3.630101f);
                manager.transform.position = new Vector3(43, 0, -5);
                break;
            default: throw new NotImplementedException();
        }
        manager.store.parkingLot = go;
    }

    public static GameObject BuildFloor(string type, string field, string square)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Floors", type)) as GameObject, GetSquare(GetField(field), square));

        go.name = go.name.Replace("(Clone)", "");
        manager.store.fields[field].squares[square].floor = go;
        if (!loading)
            GameManager.Instance.baker.enabled = true;
        return go;
    }

    public static GameObject BuildWall(string type, string field, string square, SquarePositions side)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Walls", type)) as GameObject, GetSquare(GetField(field), square));
        go.name = go.name.Replace("(Clone)", "");

        if (type.EndsWith("Wall"))
            GetWallPos(go, side);
        if (type.EndsWith("Door"))
            GetDoorPos(go, side);

        switch (side)
        {
            case SquarePositions.Xm:
                manager.store.fields[field].squares[square].wallXm = go;
                break;
            case SquarePositions.Xp:
                manager.store.fields[field].squares[square].wallXp = go;
                break;
            case SquarePositions.Zm:
                manager.store.fields[field].squares[square].wallZm = go;
                break;
            case SquarePositions.Zp:
                manager.store.fields[field].squares[square].wallZp = go;
                break;
        }
        return go;
    }

    public static void BuildCeiling(string type, string field, string square)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Ceilings", type)) as GameObject, GetSquare(GetField(field), square));
        go.name = go.name.Replace("(Clone)", "");

        manager.store.fields[field].squares[square].ceiling = go;
    }

    public static Vector3 SquareToPos(string square)
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

    public static void GetWallPos(GameObject go, SquarePositions side)
    {
        switch (side)
        {
            case SquarePositions.Xp:
                go.transform.localPosition += new Vector3(1, 1.75f, -.5f);
                go.transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case SquarePositions.Xm:
                go.transform.localPosition += new Vector3(0, 1.75f, -.5f);
                go.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case SquarePositions.Zp:
                go.transform.localPosition += new Vector3(.5f, 1.75f, 0);
                go.transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case SquarePositions.Zm:
                go.transform.localPosition += new Vector3(.5f, 1.75f, -1);
                break;
        }
    }

    private static void GetDoorPos(GameObject go, SquarePositions side)
    {
        switch (side)
        {
            case SquarePositions.Xp:
                go.transform.localPosition += new Vector3(1, .75f, -1.5f);
                go.transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case SquarePositions.Xm:
                go.transform.localPosition += new Vector3(0, .75f, .5f);
                go.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case SquarePositions.Zp:
                go.transform.localPosition += new Vector3(1.5f, .75f, 0);
                go.transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case SquarePositions.Zm:
                go.transform.localPosition += new Vector3(-.5f, .75f, -1);
                break;
        }
    }

    public static string IntToFieldString(int i)
    {
        return ((FieldLetter)Mathf.FloorToInt(i / 8f)).ToString() + (i % 8 + 1);
    }

    public static string IntToSquareString(int i)
    {
        return ((SquareLetter)Mathf.FloorToInt(i / 5f)).ToString() + (i % 5 + 1);
    }

    public static int StringToFieldInt(string s)
    {
        print(s);
        return (s[0] - 65) * 8 + int.Parse(s[1].ToString()) - 1;
    }

    public static int StringToSquareInt(string s)
    {
        return (s[0] - 65) * 5 + int.Parse(s[1].ToString()) - 1;
    }

    public static Transform GetField(string name)
    {
        foreach (Transform row in manager.transform)
            if (row.name == name[0].ToString())
                foreach (Transform field in row)
                    if (field.name == name[1].ToString())
                        return field;
        return null;
    }

    public static Transform GetSquare(Transform field, string name)
    {
        foreach (Transform square in field)
            if (square.name == name)
                return square;
        return null;
    }
}
