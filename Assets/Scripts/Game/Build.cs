using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Build : MonoBehaviour
{
    static StoreManager manager;
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
        Xpos,
        Xneg,
        Zpos,
        Zneg
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
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Floors", type)) as GameObject, GetSquare(GetField()));
        go.name = go.name.Replace("(Clone)", "") + "Floor";
        return go;
    }

    public static GameObject BuildWall(Transform square, string type, SquarePositions side)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Walls", type)) as GameObject, square);

        if (type.EndsWith("Wall"))
            GetWallPos(go, side);
        if (type.EndsWith("Door"))
            GetDoorPos(go, side);
        return go;
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
            case SquarePositions.Xpos:
                go.transform.localPosition += new Vector3(1, 1.75f, -.5f);
                go.transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case SquarePositions.Xneg:
                go.transform.localPosition += new Vector3(0, 1.75f, -.5f);
                go.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case SquarePositions.Zpos:
                go.transform.localPosition += new Vector3(.5f, 1.75f, 0);
                go.transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case SquarePositions.Zneg:
                go.transform.localPosition += new Vector3(.5f, 1.75f, -1);
                break;
        }
    }

    private static void GetDoorPos(GameObject go, SquarePositions side)
    {
        switch (side)
        {
            case SquarePositions.Xpos:
                go.transform.localPosition += new Vector3(1, .75f, -1.5f);
                go.transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case SquarePositions.Xneg:
                go.transform.localPosition += new Vector3(0, .75f, .5f);
                go.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case SquarePositions.Zpos:
                go.transform.localPosition += new Vector3(1.5f, .75f, 0);
                go.transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case SquarePositions.Zneg:
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
