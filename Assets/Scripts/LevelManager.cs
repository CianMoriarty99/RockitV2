using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    TimeTrial,
    SnakeApple,
}

public class LevelManager : MonoBehaviour
{
    public Vector3 target;

    public bool[,] levelUnlockedStatus = new bool[5, 5] 
    {
        {true,false,false,false,false},
        {false,false,false,false,false}, 
        {false,false,false,false,false},
        {false,false,false,false,false},
        {false,false,false,false,false} 
    };

    // public bool[,] levelUnlockedStatus = new bool[5, 5] 
    // {
    //     {true,true,true,true,true},
    //     {true,true,true,true,true}, 
    //     {true,true,true,true,true},
    //     {true,true,true,true,true},
    //     {true,true,true,true,true} 
    // };


    public float spaceBetweenLevels = 10f;
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    // 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        MoveTowardsTarget();
    }

    public void SetNewTargetLevel(Vector2 t)
    {
        target = new Vector2 (t.x * spaceBetweenLevels * -1, t.y * spaceBetweenLevels * -1); 
    }

    void MoveTowardsTarget()
    {
        float dist = Vector3.Distance(target, transform.position);
        if(dist > 0.01f)
        {
            float step = 20f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);
        }
    }
}
