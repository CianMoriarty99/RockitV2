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
    public float currentRunTime, timeSpentOnTheHill;
    public GameObject playerPrefab, player;
    public GameObject UIMenu;

    public GameObject currentSnakeApple, snakeApplePrefab;

    private float timeSinceLastRestart, snakeAppleCooldown;
    public GameObject ppVolume;
    public Vector2 levelSelected; // basically a tuple
    public bool countdownClock, readyForNextLevel;
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public Vector2 appleSpawnPosition;

    public int snakeScore;


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
        CloudSaveManager.Instance.LoadFromJson();
        TimeSpan time = TimeSpan.FromSeconds(bestTimes[0,0]);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));

        timeSpentOnTheHill = 0;
        levelSelected = new Vector2(0,0);
        countdownClock = false;
        timeSinceLastRestart = 0;
        snakeAppleCooldown = 2;
        appleSpawnPosition = new Vector2(0,1);
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownClock && (levelSelected.y == 0 || levelSelected.y == 1)) //Don't countdown for snake or KotH
        {
            currentRunTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentRunTime);
            lastTimeText.SetText(time.ToString("m':'ss'.'fff"));
        }
        else if(countdownClock && (levelSelected.y == 2 || (levelSelected.y == 4 && (levelSelected.x == 1 || levelSelected.x == 3)))) //Snake
        {
            SpawnSnakeApples();
            currentRunTime = snakeScore;
            TimeSpan time = TimeSpan.FromSeconds(currentRunTime);
            lastTimeText.SetText(time.ToString("m':'ss'.'fff"));
        }
        else if(countdownClock && (levelSelected.y == 3 || (levelSelected.y == 4 && (levelSelected.x == 0 || levelSelected.x == 2 || levelSelected.x == 4)))) //KotH
        {
            
            currentRunTime = timeSpentOnTheHill;
            TimeSpan time = TimeSpan.FromSeconds(currentRunTime);
            lastTimeText.SetText(time.ToString("m':'ss'.'fff"));
        }

        timeSinceLastRestart += Time.deltaTime;
    }

    public void SpawnSnakeApples()
    {
        if(currentSnakeApple == null)
        {
            snakeAppleCooldown -= Time.deltaTime;

            if(snakeAppleCooldown < 0)
            {
                appleSpawnPosition = appleSpawnPosition * -1;
                snakeAppleCooldown = 2f;
                currentSnakeApple = Instantiate(snakeApplePrefab, appleSpawnPosition, Quaternion.identity);
            }
        }
    }

    public void RestartLevel()
    {
        if(timeSinceLastRestart > 1)
        {
            timeSpentOnTheHill = 0;
            Destroy(currentSnakeApple);
            snakeAppleCooldown = 2f;
            snakeScore = 0;
            timeSinceLastRestart = 0;
            currentRunTime = 0f;
            countdownClock = true;
            Vector3 pos = new Vector3(0,0,0);
            player = Instantiate(playerPrefab, pos, Quaternion.identity);
            UiManager.Instance.setNewPlayerCameraTarget(player);
        }

    }

    public void StopTimer()
    {

        if(currentSnakeApple) Destroy(currentSnakeApple);

        countdownClock = false;
        Debug.Log(lastTimes);
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

        if(newBestTime >= 5) 
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

        CloudSaveManager.Instance.SaveToJson();
    }


    public void moveLevelUp()
    {
        if(levelSelected.y < 4)
        {
            levelSelected = new Vector2(levelSelected.x, levelSelected.y + 1);
            LevelManager.Instance.SetNewTargetLevel(levelSelected);
            UiManager.Instance.SetNewSelectedLevel(levelSelected);
            SoundEffectPlayer.Instance.playSwitchLevelClip();
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
            SoundEffectPlayer.Instance.playSwitchLevelClip();
        } 

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        Debug.Log(bestTimeForLevel);
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
            SoundEffectPlayer.Instance.playSwitchLevelClip();
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
            SoundEffectPlayer.Instance.playSwitchLevelClip();
        }

        float bestTimeForLevel = bestTimes[(int)levelSelected.x,(int)levelSelected.y];
        TimeSpan time = TimeSpan.FromSeconds(bestTimeForLevel);
        bestTimeText.SetText(time.ToString("m':'ss'.'fff"));
        lastTimeText.SetText("0:00.000");

    }

    void InitArrays()
    {
        lastTimes = new float[5,5];
    }
}
