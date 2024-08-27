using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PulpitManager : MonoBehaviour
{
    public GameObject pulpitPrefab;
    private List<GameObject> pulpits = new List<GameObject>();
    private float minDestroyTime;
    private float maxDestroyTime;
    private float spawnTime;

    void Start()
    {
        LoadPulpitData();
        StartCoroutine(SpawnPulpits());
    }

    IEnumerator SpawnPulpits()
    {
        while (true)
        {
            if (pulpits.Count < 2)
            {
                Vector3 spawnPosition = GetRandomPosition();
                GameObject newPulpit = Instantiate(pulpitPrefab, spawnPosition, Quaternion.identity);
                pulpits.Add(newPulpit);
                StartCoroutine(DestroyPulpit(newPulpit));
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator DestroyPulpit(GameObject pulpit)
    {
        yield return new WaitForSeconds(Random.Range(minDestroyTime, maxDestroyTime));
        pulpits.Remove(pulpit);
        Destroy(pulpit);
    }

    Vector3 GetRandomPosition()
    {
        if (pulpits.Count == 0) return new Vector3(0, 0, 0);

        Vector3 lastPulpitPosition = pulpits[pulpits.Count - 1].transform.position;
        Vector3 newPosition = lastPulpitPosition + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        return newPosition;
    }

    void LoadPulpitData()
    {
        string path = Application.dataPath + "/DoofusDiary.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PulpitData pulpitData = JsonUtility.FromJson<PulpitData>(jsonData);
            minDestroyTime = pulpitData.pulpit_data.min_pulpit_destroy_time;
            maxDestroyTime = pulpitData.pulpit_data.max_pulpit_destroy_time;
            spawnTime = pulpitData.pulpit_data.pulpit_spawn_time;
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }

    [System.Serializable]
    public class PulpitData
    {
        public PulpitTiming pulpit_data;

        [System.Serializable]
        public class PulpitTiming
        {
            public float min_pulpit_destroy_time;
            public float max_pulpit_destroy_time;
            public float pulpit_spawn_time;
        }
    }
}
