using UnityEngine;
using System.Collections;
using System.IO;

public class DoofusMovement : MonoBehaviour
{
    public float speed = 3.0f;

    void Start()
    {
        LoadPlayerData();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, 0, moveZ));
    }

    void LoadPlayerData()
    {
        string path = Application.dataPath + "/DoofusDiary.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            speed = playerData.player_data.speed;
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public PlayerSpeed player_data;

        [System.Serializable]
        public class PlayerSpeed
        {
            public float speed;
        }
    }
}
