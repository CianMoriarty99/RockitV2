using UnityEngine;
using Steamworks;
using System.IO;

public class CloudSaveManager : MonoBehaviour
{
    private static CloudSaveManager _instance;
    public static CloudSaveManager Instance { get { return _instance; } }
    public string saveFileName = "SaveFile.json";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void SaveToJson()
    {
        SaveData save = new SaveData();
        save.bestTimes = FlattenArray(GameManager.Instance.bestTimes);
        save.levelUnlockedStatus = FlattenArray(LevelManager.Instance.levelUnlockedStatus);

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), json);
    }

    public void LoadFromJson()
    {
        SaveData data;
        if(File.Exists(Path.Combine(Application.persistentDataPath, saveFileName)))
        {
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, saveFileName));
            data = JsonUtility.FromJson<SaveData>(json);

        } else 
        {
            // Create a new save file
            data = new SaveData();

            data.bestTimes = new float[25];
            data.levelUnlockedStatus = new bool[25];

            // Set all elements to false initially
            for (int i = 0; i < data.levelUnlockedStatus.Length; i++)
            {
                data.bestTimes[i] = 0.0f;
            }

            // Set all elements to false initially
            for (int i = 1; i < data.levelUnlockedStatus.Length; i++)
            {
                data.levelUnlockedStatus[i] = false;
            }

            // Set the first element to true
            data.levelUnlockedStatus[0] = true;

            data.levelUnlockedStatus[0] = true;
            // Initialize newData with default values if needed

            string newJson = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), newJson);

            Debug.Log("New save file created at: " + Path.Combine(Application.persistentDataPath, saveFileName));
        }

        if(data.bestTimes != null)
        {
            GameManager.Instance.bestTimes = UnflattenArray(data.bestTimes, 5 , 5);
        }

        if(data.levelUnlockedStatus != null)
        {
            LevelManager.Instance.levelUnlockedStatus = UnflattenArray(data.levelUnlockedStatus, 5 , 5);
        }

    }

    // Helper method to flatten 2D arrays into 1D arrays
    private T[] FlattenArray<T>(T[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        T[] flattenedArray = new T[rows * cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                flattenedArray[i * cols + j] = array[i, j];
            }
        }

        return flattenedArray;
    }

    private T[,] UnflattenArray<T>(T[] flattenedArray, int rows, int cols)
    {
        T[,] array2D = new T[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array2D[i, j] = flattenedArray[i * cols + j];
            }
        }

        return array2D;
    }
}