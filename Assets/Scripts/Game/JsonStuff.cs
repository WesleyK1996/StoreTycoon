using Newtonsoft.Json;
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
            {
                print(content);
                LoadGame(JsonUtility.FromJson<StoreData>(content));
            }
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

    public IEnumerator SaveGame(StoreData data, bool format)
    {
        print("saving");
        yield return new WaitWhile(() => SaveInProgress);
        SaveInProgress = true;
        string s = data.name + "\n";
        //need to convert the damn store to Json in a way thats not too ridiculous
        //need to store every string AND deserialize all dictionaries
        s += store. "\n";



        if (format)
            s = ConvertJsonToReadableString(s);
        print(s);
        SaveInProgress = false;
        yield return null;
    }

    public static IEnumerator SaveJsonFile(Store store, bool MakePretty)
    {
        print("breaking out of save");
        yield break;
        print("saving");
        yield return new WaitUntil(() => !SaveInProgress);
        SaveInProgress = true;
        //string s = JsonConvert.DeserializeObject<Dictionary<string, StoreData>>(StoreToStoreData(store));
        if (MakePretty)
            s = ConvertJsonToReadableString(s);
        print(s);

        using (TextWriter tw = new StreamWriter(FilePath, append: false))
            tw.WriteLine(s);
        SaveInProgress = false;
        yield return null;
    }

    public void LoadGame(StoreData data)
    {
        print("loading");
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
                if (data.fields[field.field.name].squares[square.self.name].floor != "")
                    Build.BuildFloor(data.fields[field.field.name].squares[square.self.name].floor, field.name, square.self.name);
                if (data.fields[field.field.name].squares[square.self.name].wallXp != "")
                    Build.BuildWall(data.fields[field.field.name].squares[square.self.name].wallXp, field.field.name, square.self.name, Build.SquarePositions.Xp);
                if (data.fields[field.field.name].squares[square.self.name].wallXm != "")
                    Build.BuildWall(data.fields[field.field.name].squares[square.self.name].wallXm, field.field.name, square.self.name, Build.SquarePositions.Xm);
                if (data.fields[field.field.name].squares[square.self.name].wallZp != "")
                    Build.BuildWall(data.fields[field.field.name].squares[square.self.name].wallZp, field.field.name, square.self.name, Build.SquarePositions.Zp);
                if (data.fields[field.field.name].squares[square.self.name].wallZm != "")
                    Build.BuildWall(data.fields[field.field.name].squares[square.self.name].wallZm, field.field.name, square.self.name, Build.SquarePositions.Zm);
                if (data.fields[field.field.name].squares[square.self.name].ceiling != "")
                    Build.BuildCeiling(data.fields[field.field.name].squares[square.self.name].ceiling, field.field.name, square.self.name);
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
        {
            print(FindObjectOfType<StoreManager>().store.name);
            StartCoroutine(SaveJsonFile(FindObjectOfType<StoreManager>().store, true));
        }
    }
}
