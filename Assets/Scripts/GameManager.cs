using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public float [,] bestTimes, lastTimes;
    public float currentRunTime;
    public GameObject playerPrefab;
    public GameObject UIMenu;

    private float timeSinceLastRestart;
    public GameObject ppVolume;
    public Vector2 levelSelected; // basically a tuple
    private bool countdownClock;
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitArrays();
        levelSelected = new Vector2(0,0);
        countdownClock = false;
        timeSinceLastRestart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownClock)
        {
            currentRunTime += Time.deltaTime;
        }

        timeSinceLastRestart += Time.deltaTime;
    }

    public void RestartLevel()
    {
        if(timeSinceLastRestart > 1)
        {
            timeSinceLastRestart = 0;
            currentRunTime = 0f;
            countdownClock = true;
            Vector3 pos = new Vector3(0,0,0);
            GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
            UiManager.Instance.setNewPlayerCameraTarget(player);
        }

    }

    public void StopTimer()
    {
        countdownClock = false;
        lastTimes[(int)levelSelected.x,(int)levelSelected.y] = currentRunTime;

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        if(currentRunTime > bestTimeForLevel)
        {
            bestTimes[(int)levelSelected.x,(int)levelSelected.y] = currentRunTime;
        }
        //Show new best time thing
    }


    public void moveLevelUp()
    {
        if(levelSelected.y < 4)
        {
            levelSelected = new Vector2(levelSelected.x, levelSelected.y + 1);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }
    }

    public void moveLevelDown()
    {
        if(levelSelected.y > 0)
        {
            levelSelected = new Vector2(levelSelected.x, levelSelected.y - 1);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        } 
    }

    public void moveLevelLeft()
    {
        if(levelSelected.x > 0)
        {
            levelSelected = new Vector2(levelSelected.x - 1, levelSelected.y);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }
    }

    public void moveLevelRight()
    {
        if(levelSelected.x < 4)
        {
            levelSelected = new Vector2(levelSelected.x + 1, levelSelected.y);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }


    }

    void InitArrays()
    {
        bestTimes = new float[5,5];
        lastTimes = new float[5,5];
    }
}
