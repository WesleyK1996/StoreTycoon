using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Build : MonoBehaviour
{
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


    public void SpawnParkingLot(int type)
    {
        Destroy(GameObject.FindGameObjectWithTag("Parking"));
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Parking", "Parking" + type)) as GameObject);
        switch (go.name.Replace("Parking", "").Replace("(Clone)", ""))
        {
            case "1":
                go.transform.position = new Vector3(16.66694f, -1.605933f, -3.630101f);
                StoreManager.Instance.transform.position = new Vector3(43, 0, -5);
                break;
            default: throw new NotImplementedException();
        }
        StoreManager.Instance.store.parkingLot = "Parking" + type;
        StartCoroutine(StoreManager.Instance.data.SaveJsonFile(StoreManager.Instance.store, true));
    }

    public void BuildFloor(Transform field, string square , string type)
    {
        GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Floors", type)) as GameObject, field.Find(square));
        go.transform.localPosition = SquareToPos(square);
        for (int i = 0; i < StoreManager.Instance.store.fields.Count; i++)
            if (StoreManager.Instance.store.fields[i].field == field.gameObject)
                for (int j = 0; j < StoreManager.Instance.store.fields[i].squares.Count; j++)
                    StoreManager.Instance.store.fields[i].squares[j].floor = go;
    }

    public void BuildWall(Transform field, string square, string type, SquarePositions side)
    {
        //GameObject go = Instantiate(Resources.Load(Path.Combine("Prefabs", "Walls", type)) as GameObject, field.Find(square));
        //Vector3 adjustment = Vector3.zero;
        //switch (side)
        //{
        //    case SquarePositions.Xpos:

        //        break;
        //    case SquarePositions.Xneg:
        //        break;
        //    case SquarePositions.Zpos:
        //        break;
        //    case SquarePositions.Zneg:
        //        break;
        //}

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

    public static string FieldName(int i)
    {
        return ((FieldLetter)Mathf.FloorToInt(i / 8f)).ToString() + (i % 8 + 1);
    }

    public static string SquareName(int i)
    {
        return ((SquareLetter)Mathf.FloorToInt(i / 5f)).ToString() + (i % 5 + 1);
    }
}
