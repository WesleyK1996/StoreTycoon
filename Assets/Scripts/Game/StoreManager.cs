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
    public List<GameObject> advertising = new List<GameObject>();
    public Dictionary<string,Field> fields = new Dictionary<string,Field>();

    [Serializable]
    public class Field
    {
        public string name;
        public GameObject field;
        public Dictionary<string,Square> squares = new Dictionary<string, Square>();

        [Serializable]
        public class Square
        {
            public GameObject self;
            public GameObject floor;
            //walls on the plus and minus x and z sides of the square 
            public GameObject wallXp;
            public GameObject wallXm;
            public GameObject wallZp;
            public GameObject wallZm;
            public GameObject ceiling;
            public GameObject hangable;
            public GameObject furniture;//needs custom furniture class
        }
    }
}

public class StoreManager : MonoBehaviour
{
    public Store store = new Store();
}
