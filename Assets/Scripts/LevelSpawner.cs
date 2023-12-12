using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject levelPrefab, doorPrefab;
    public int maxRows, maxColumns, distanceBetweenLevels;
    public float distanceToDoorEdge = 1.345f;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateLevels(levelPrefab, doorPrefab, maxRows, maxColumns, distanceBetweenLevels, distanceToDoorEdge);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateLevels(GameObject level, GameObject door, int rows, int columns, int distanceBetweenLevels, float distanceToDoorEdge)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 levelPosition = new Vector3 (j * distanceBetweenLevels, i * distanceBetweenLevels, 0); // j = x, i = y
                
                GameObject room = Instantiate(level, levelPosition, Quaternion.identity, this.gameObject.transform);

                //left door
                if( j > 0 )
                {
                    Vector3 doorPosition = new Vector3(levelPosition.x - distanceToDoorEdge, levelPosition.y, levelPosition.z);
                    Instantiate(door, doorPosition,  Quaternion.AngleAxis(-90, new Vector3(0,0,1)), room.transform);
                }

                //right door
                if( j < rows - 1 )
                {
                    Vector3 doorPosition = new Vector3(levelPosition.x + distanceToDoorEdge, levelPosition.y, levelPosition.z);
                    Instantiate(door, doorPosition,  Quaternion.AngleAxis(90, new Vector3(0,0,1)), room.transform);
                }
                //bottom door
                if( i > 0 )
                {
                    Vector3 doorPosition = new Vector3(levelPosition.x, levelPosition.y - distanceToDoorEdge, levelPosition.z);
                    Instantiate(door, doorPosition, Quaternion.identity, room.transform);
                }
                //top door
                if( i < columns - 1)
                {
                    Vector3 doorPosition = new Vector3(levelPosition.x, levelPosition.y + distanceToDoorEdge, levelPosition.z);
                    Instantiate(door, doorPosition, Quaternion.AngleAxis(180, new Vector3(0,0,1)),  room.transform);
                }
                
                
            }
        }
    }
}
