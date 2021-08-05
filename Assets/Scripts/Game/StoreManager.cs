using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Store
{
    public string name;
    public int money;
    public GameObject parkingLot;
    public List<GameObject> advertising;
    public List<Field> fields;

    [Serializable]
    public class Field
    {
        public string name;
        public GameObject field;
        public List<Square> squares;

        [Serializable]
        public class Square
        {
            public string name;
            public GameObject floor;
            //walls on the plus and minus x and z sides of the square 
            public GameObject wallXp;
            public GameObject wallXm;
            public GameObject wallZp;
            public GameObject wallZm;
            public GameObject ceiling;
            public GameObject ceilingLight;
            public GameObject furniture;
        }
    }
}

public class StoreManager : MonoBehaviour
{
    public Build build;

    public Store store = new Store();

    private void OnEnable()
    {
        build = GetComponent<Build>();
    }

    void Start()
    {
       
    }
}
