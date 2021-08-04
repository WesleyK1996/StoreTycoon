using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Store
{
    public string name;
    public GameObject parkingLot;
    public List<GameObject> advertising;
    public Field[,] fields = new Field[8, 8]; 

    [Serializable]
    public class Field
    {
        public string name;
        public Square[,] squares = new Square[5, 5];

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
    public Store store = new Store();

    void Start()
    {
        StartCoroutine(SpawnSavedFields());
    }

    private IEnumerator SpawnSavedFields()
    {
        string[] letters = "A,B,C,D,E,F,G,H".Split(',');
        string[] numbers = "1,2,3,4,5,6,7,8".Split(',');

        for (int x = 0; x < letters.Length; x++)
        {
            for (int y = 0; y < numbers.Length; y++)
            {
                if (PlayerPrefs.HasKey(letters[x] + numbers[y] + "owned"))
                    MakeField();
            }
        }

        yield return null;
    }

    private void MakeField()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
