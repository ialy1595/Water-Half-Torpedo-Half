using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [HideInInspector] public bool isPaused;

    [HideInInspector] public float gameTime;
    private float gameStartTime;
    private float gamePauseTime;
    private float pauseStartTime;
    private bool pauseStart;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.sm = this;
        isPaused = false;
        gameTime = 0;
        gamePauseTime = 0;
        gameStartTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
        TimeUpdate();
    }

    void TimeUpdate()
    {
        gameTime = Time.time - gameStartTime - gamePauseTime;
    }
}
