using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class JsonStuff : MonoBehaviour
{
    string FilePath;
    string Dir;
    bool SaveInProgress;

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
        print("loading");
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        yield return new WaitUntil(() => Directory.Exists(Dir));
        if (!File.Exists(FilePath))
        {
            CreateNewGame();
            yield return null;
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
                StoreManager.Instance.store = JsonUtility.FromJson<Store>(content);
        }
    }

    private void CreateNewGame()
    {
        //FileStream FS = File.Create(FilePath);
        //FS.Close();
        gameObject.AddComponent<NewGame>();
    }

    public IEnumerator SaveJsonFile(Store store, bool MakePretty)
    {
        print("saving");
        yield return new WaitWhile(() => SaveInProgress);
        SaveInProgress = true;
        string s = JsonUtility.ToJson(store);
        if (MakePretty)
            s = ConvertJsonToReadableString(s);
        print(s);

        using (TextWriter tw = new StreamWriter(FilePath, append: false))
            tw.WriteLine(s);
        SaveInProgress = false;
    }

    string ConvertJsonToReadableString(string s)
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
}
