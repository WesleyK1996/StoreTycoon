using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class StoreData
{
    public string name;
    public int money;
    public int parkingLot;
    public List<string> advertising;
    public Dictionary<string, FieldData> fields = new Dictionary<string, FieldData>();

    [Serializable]
    public class FieldData
    {
        public string name;
        public Dictionary<string, SquareData> squares = new Dictionary<string, SquareData>();

        [Serializable]
        public class SquareData
        {
            public string name;
            public string floor;
            //walls on the plus and minus x and z sides of the square 
            public string wallXp;
            public string wallXm;
            public string wallZp;
            public string wallZm;
            public string ceiling;
            public string hangable;
            public string furniture;//needs custom furniture class
        }
    }
}
//with a full store theres about 5600 gameobjects, this is without products on the shelves
//might need to look for something more efficient
//LODs might help (slightly)
//I could always decrease the amount of fields(currently 8x8) or decrease squares per field(currently 5x5)

public class JsonStuff : MonoBehaviour
{
    public static JsonStuff Instance;

    static string FilePath;
    string Dir;
    static bool SaveInProgress;
    public bool delete;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        Dir = Application.persistentDataPath + "/JsonFiles/";
        FilePath = Dir + "JsonFile.Json";
        StartCoroutine(LoadLocalizedText());
    }

    /// <summary>
    /// Load settings from json file. (Android friendly)
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLocalizedText()
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        yield return new WaitUntil(() => Directory.Exists(Dir));
        if (!File.Exists(FilePath))
        {
            CreateNewGame();
            yield break;
        }
        print("loading");
        yield return new WaitUntil(() => File.Exists(FilePath));

        bool FailedRead = false;
        string content = "";
        if (FilePath.Contains("://") || FilePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(FilePath);
            yield return www.SendWebRequest();
            try
            { content = www.downloadHandler.text; }
            catch
            { FailedRead = true; }
        }
        else
        {
            try
            {
                content = File.ReadAllText(FilePath);
            }
            catch
            {
                FailedRead = true;
            }
        }

        if (FailedRead)
            throw new FileLoadException();
        else
        {
            if (content != "")
                LoadGame(JsonUtility.FromJson<StoreData>(content));
            else
                CreateNewGame();
        }
    }

    private void CreateNewGame()
    {
        print("newgame");
        FileStream FS = File.Create(FilePath);
        FS.Close();
        gameObject.AddComponent<NewGame>();
    }

    public static StoreData StoreToStoreData(Store store)
    {
        StoreData storeData = new StoreData();
        storeData.name = store.name;
        storeData.money = store.money;
        storeData.parkingLot = int.Parse(store.parkingLot.name.Replace("Parking", ""));
        foreach (GameObject go in store.advertising)
            storeData.advertising.Add(go.name);
        foreach (Store.Field f in store.fields.Values)
        {
            StoreData.FieldData field = new StoreData.FieldData();
            field.name = f.name;
            foreach (Store.Field.Square s in f.squares.Values)
            {
                StoreData.FieldData.SquareData square = new StoreData.FieldData.SquareData();
                square.name = s.self.name;
                if (s.floor != null)
                    square.floor = s.floor.name;
                else square.floor = null;
                if (s.wallXp != null)
                    square.wallXp = s.wallXp.name;
                else square.wallXp = null;
                if (s.wallXm != null)
                    square.wallXm = s.wallXm.name;
                else square.wallXm = null;
                if (s.wallZp != null)
                    square.wallZp = s.wallZp.name;
                else square.wallZp = null;
                if (s.wallZm != null)
                    square.wallZm = s.wallZm.name;
                else square.wallZm = null;
                if (s.ceiling != null)
                    square.ceiling = s.ceiling.name;
                if (s.hangable != null)
                    square.hangable = s.hangable.name;
                else square.hangable = null;
                if (s.furniture != null)
                    square.furniture = s.furniture.name;
                else square.furniture = null;

                field.squares.Add(square.name, square);
            }
            storeData.fields.Add(field.name, field);
        }
        return storeData;
    }

    public static IEnumerator SaveJsonFile(Store store, bool MakePretty)
    {
        print("saving");
        yield return new WaitUntil(() => !SaveInProgress);
        SaveInProgress = true;
        string s = JsonUtility.ToJson(StoreToStoreData(store));
        if (MakePretty)
            s = ConvertJsonToReadableString(s);

        using (TextWriter tw = new StreamWriter(FilePath, append: false))
            tw.WriteLine(s);
        SaveInProgress = false;
        yield return null;
    }

    public void LoadGame(StoreData data)
    {
        Store store = Instantiate(Resources.Load(Path.Combine("Prefabs", "Store")) as GameObject).GetComponent<StoreManager>().store;
        store.name = data.name;
        store.money = data.money;
        Build.BuildParkingLot(data.parkingLot);

        for (int i = 0; i < 64; i++)
        {
            Store.Field field = new Store.Field();
            field.name = Build.IntToFieldString(i);
            field.field = Build.GetField(Build.IntToFieldString(i)).gameObject;
            for (int j = 0; j < 25; j++)
            {
                Store.Field.Square square = new Store.Field.Square();
                square.self = new GameObject() { name = Build.IntToSquareString(j) };
                square.self.transform.parent = field.field.transform;
                square.self.transform.localPosition = Build.SquareToPos(square.self.name);
                if (data.fields[i].squares[j].floor != "")
                    Build.BuildFloor(data.fields[i].squares[j].floor, field.name, square.self.name);
                if (data.fields[i].squares[j].wallXp != "")
                    Build.BuildWall(square.self.transform, data.fields[i].squares[j].wallXp, Build.SquarePositions.Xpos);
                //the other stuffs
                field.squares.Add(square.self.name, square);
            }
            store.fields.Add(field.name, field);
        }
        print("LOADING INCOMPLETE");
    }

    static string ConvertJsonToReadableString(string s)
    {
        string tabs = "";

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '{' || s[i] == '[')
            {
                tabs += "  ";
                s = s.Insert(i + 1, "\n" + tabs);
                i += tabs.Length + 1;
            }
            else if (s[i] == '}' || s[i] == ']')
            {
                tabs = tabs.Remove(tabs.Length - 2);
                s = s.Insert(i, "\n" + tabs);
                i += tabs.Length + 1;
            }
            else if (s[i] == ',')
            {
                s = s.Insert(i + 1, "\n" + tabs);
                i += tabs.Length + 1;
            }
        }
        return s;
    }

    private void OnApplicationQuit()
    {
        if (!delete)
            StartCoroutine(SaveJsonFile(StoreToStoreData(FindObjectOfType<StoreManager>().store), true));
    }
}
