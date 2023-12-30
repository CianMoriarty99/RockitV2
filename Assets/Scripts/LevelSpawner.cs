using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] obstacles; //0 is for (0,0), 25 is for (4,4)
    public GameObject levelPrefab;
    public int maxRows, maxColumns, distanceBetweenLevels;


    // Start is called before the first frame update
    void Awake()
    {
        GenerateLevels(levelPrefab, obstacles, maxRows, maxColumns, distanceBetweenLevels);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateLevels(GameObject level, GameObject[] obstacles, int rows, int columns, int distanceBetweenLevels)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 levelPosition = new Vector3 (j * distanceBetweenLevels, i * distanceBetweenLevels, 0); // j = x, i = y
                
                GameObject room = Instantiate(level, levelPosition, Quaternion.identity, this.gameObject.transform);

                int index = i*5 + j;

                if(obstacles[index])
                {
                    GameObject obstacle = Instantiate(obstacles[index], levelPosition, Quaternion.identity, this.gameObject.transform);
                }

            }
        }
    }
}
