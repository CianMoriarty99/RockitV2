using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] obstacles; //0 -> 4 repeated for each row
    public GameObject lasers; // repeated for each row
    public GameObject koth; //repeated for each row
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

                if(i == 1 || i == 4)
                {
                    GameObject laserObj = Instantiate(lasers, levelPosition, Quaternion.identity, this.gameObject.transform);
                }

                if(i == 3 || (i == 4 && (j == 0 || j == 2 || j == 4)))
                {
                    GameObject kothObj = Instantiate(koth, levelPosition, Quaternion.identity, this.gameObject.transform);
                }

                if(obstacles[j])
                {
                    if((j == 0 && i == 0 ) || j > 0)
                    {
                        //for some reason the placement is dodgy
                        Vector3 newPosition = new Vector3(levelPosition.x - 0.025f, levelPosition.y + 0.025f, levelPosition.z);
                        GameObject obstacle = Instantiate(obstacles[j], newPosition, Quaternion.identity, this.gameObject.transform);


                    }

                }
            }
        }
    }
}
