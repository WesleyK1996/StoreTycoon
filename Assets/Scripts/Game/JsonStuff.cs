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
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        yield return new WaitUntil(() => Directory.Exists(Dir));
        if (!File.Exists(FilePath))
        {
            FileStream FS = File.Create(FilePath);
            FS.Close();
            yield return StartCoroutine(SaveJsonFile(new JsonData(), true));
        }
        yield return new WaitUntil(() => File.Exists(FilePath));

        bool FailedRead = false;
        if (FilePath.Contains("://") || FilePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(FilePath);
            yield return www.SendWebRequest();
            try
            { JsonFile = JsonUtility.FromJson<JsonData>(www.downloadHandler.text); }
            catch
            { FailedRead = true; }
        }
        else
        {
            try
            { JsonFile = JsonUtility.FromJson<JsonData>(FilePath); }
            catch
            { }
        }

        if (FailedRead)
        {
            throw new FileLoadException();
        }
    }

    IEnumerator SaveJsonFile(StoreManager.Field , bool MakePretty)
    {
        yield return new WaitWhile(() => SaveInProgress);
        SaveInProgress = true;
        string s = JsonUtility.ToJson(Js);
        if (MakePretty)
            s = ConvertJsonToReadableString(s);

        using (TextWriter tw = new StreamWriter(FilePath, append: false))
            tw.WriteLine(s);
        SaveInProgress = false;
    }

    //IEnumerator SaveJsonFile(JsonData Js, bool MakePretty)
    //{
    //    yield return new WaitWhile(() => SaveInProgress);
    //    SaveInProgress = true;
    //    string s = JsonUtility.ToJson(Js);
    //    if (MakePretty)
    //        s = ConvertJsonToReadableString(s);

    //    bool succes = false;
    //    while (succes == false)
    //    {
    //        using (TextWriter tw = new StreamWriter(FilePath, append: false))
    //        {
    //            succes = true;
    //            tw.WriteLine(s);
    //        }
    //        yield return new WaitForSecondsRealtime(0.1f);
    //    }
    //    SaveInProgress = false;
    //}

    //public void AddMood(string Name, string Mood, string Activity)
    //{

    //    if (JsonFile.Users == null)
    //    {
    //        JsonFile.Users = new List<JsonData.User>();
    //    }

    //    int n = 0;

    //    if (JsonFile.Users.Count > 0)
    //    {
    //        n = JsonFile.Users.FindIndex(User => User.Name == Name);
    //        if (n == -1)
    //            n = NewUser(Name);
    //    }
    //    else n = NewUser(Name);
    //    AddEntry(n, Mood, Activity);

    //    SortNames(JsonFile);
    //    StartCoroutine(SaveJsonFile(JsonFile, true));
    //}


    //private int NewUser(string Name)
    //{
    //    JsonFile.Users.Add(new JsonData.User());
    //    JsonFile.Users[JsonFile.Users.Count - 1].Name = Name;
    //    return JsonFile.Users.Count - 1;
    //}

    //void AddEntry(int Index, string Mood, string Activity)
    //{
    //    if (JsonFile.Users[Index].Entries == null)
    //    {
    //        JsonFile.Users[Index].Entries = new List<JsonData.User.Entry>();
    //    }

    //    JsonFile.Users[Index].Entries.Add(new JsonData.User.Entry());
    //    JsonFile.Users[Index].Entries[JsonFile.Users[Index].Entries.Count - 1].Date = DateTime.Now.ToString();
    //    JsonFile.Users[Index].Entries[JsonFile.Users[Index].Entries.Count - 1].Mood = Mood;
    //    JsonFile.Users[Index].Entries[JsonFile.Users[Index].Entries.Count - 1].Activity = Activity;
    //}

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
