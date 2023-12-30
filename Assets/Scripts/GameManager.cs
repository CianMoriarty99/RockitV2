using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Time keeping
    public float [,] bestTimes, lastTimes;

    public TMP_Text bestTimeText, lastTimeText;
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
            TimeSpan time = TimeSpan.FromSeconds(currentRunTime);
            lastTimeText.SetText(time.ToString("m':'ss'.'fff"));
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UiManager.Instance.PanelFadeOut();
                RestartLevel();
            }
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
            TimeSpan time = TimeSpan.FromSeconds(currentRunTime);
            bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        }
        //Show new best time thing

        //Unlock rooms
        float newBestTime = bestTimes[(int)levelSelected.x,(int)levelSelected.y];

        if(newBestTime > 5) 
        {
            if((int)levelSelected.x < 4)
                LevelManager.Instance.levelUnlockedStatus[(int)levelSelected.x + 1,(int)levelSelected.y] = true;
            
            if((int)levelSelected.y < 4)
                LevelManager.Instance.levelUnlockedStatus[(int)levelSelected.x, (int)levelSelected.y + 1] = true;

            if((int)levelSelected.x > 0) 
                LevelManager.Instance.levelUnlockedStatus[(int)levelSelected.x - 1,(int)levelSelected.y] = true;

            if((int)levelSelected.y > 0)
                LevelManager.Instance.levelUnlockedStatus[(int)levelSelected.x, (int)levelSelected.y - 1] = true;
        }
    }


    public void moveLevelUp()
    {
        if(levelSelected.y < 4)
        {
            levelSelected = new Vector2(levelSelected.x, levelSelected.y + 1);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        TimeSpan time = TimeSpan.FromSeconds(bestTimeForLevel);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        lastTimeText.SetText("0:00.000");
    }

    public void moveLevelDown()
    {
        if(levelSelected.y > 0)
        {
            levelSelected = new Vector2(levelSelected.x, levelSelected.y - 1);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        } 

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        TimeSpan time = TimeSpan.FromSeconds(bestTimeForLevel);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        lastTimeText.SetText("0:00.000");
    }

    public void moveLevelLeft()
    {
        if(levelSelected.x > 0)
        {
            levelSelected = new Vector2(levelSelected.x - 1, levelSelected.y);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        TimeSpan time = TimeSpan.FromSeconds(bestTimeForLevel);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        lastTimeText.SetText("0:00.000");
    }

    public void moveLevelRight()
    {
        if(levelSelected.x < 4)
        {
            levelSelected = new Vector2(levelSelected.x + 1, levelSelected.y);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
        }

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        TimeSpan time = TimeSpan.FromSeconds(bestTimeForLevel);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        lastTimeText.SetText("0:00.000");

    }

    void InitArrays()
    {
        bestTimes = new float[5,5];
        lastTimes = new float[5,5];

        //init with player data
    }
}
