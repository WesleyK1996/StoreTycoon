using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NavigationBaker baker;
    public StoreManager manager;

    public Transform road;

    int minCarSpawnTime = 500;
    int maxCarSpawnTime = 1000;
    float carSpawnsIn = 3f;

    string[] cars = new string[1]
    {
        "Car1Blue"
    };

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(JsonStuff.Instance.LoadLocalizedText());
        baker.enabled = true;
        if (manager.store.advertising.Count > 0)
            SetCarSpawnTime();
    }

    private void SetCarSpawnTime()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if (carSpawnsIn <= 0f)
            SpawnCar();

        carSpawnsIn -= Time.deltaTime;
    }

    private void SpawnCar()
    {
        Instantiate(Resources.Load(Path.Combine("Prefabs", "Cars", cars[Random.Range(0, cars.Length)])) as GameObject);
        carSpawnsIn = Random.Range(minCarSpawnTime, maxCarSpawnTime);
    }
}
