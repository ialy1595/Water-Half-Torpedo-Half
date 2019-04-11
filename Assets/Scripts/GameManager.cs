using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    public enum TouchState
    {
        None = 0,
        Left = 1,
        Right = 2,
    };

    public static GameManager gm;

    public GameObject Gamemanager;
    public static GameObject _Gamemanager;
    private static bool GMCreated = false;

    [HideInInspector] public bool isGaming = false;
    [HideInInspector] public bool isPaused;

    [HideInInspector] public float gameTime;
    private float gameStartTime;
    private float gamePauseTime;
    private float pauseStartTime;
    private bool pauseStart;

    [HideInInspector] public Handle myHandle;
    [HideInInspector] public Submarine mySubmarine;
    
    private TouchState nowTouchState;
    private TouchState prevTouchState = 0;
    

    void Awake()
    {
        if (GMCreated == true)  // GM 중복생성 방지
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);    // 씬이 넘어가도 파괴되지 않음

        gm = this;
        _Gamemanager = Gamemanager;
    }
    
    void Start()
    {
        if (GMCreated == true)  // GM 중복생성 방지
            return;
        GMCreated = true;

        Random.InitState((int)Time.time);
    }

    void Update()
    {
        if(isGaming)
        {
            TimeUpdate();
            CheckTouch();
        }
    }

    void TimeUpdate()
    {
        gameTime = Time.time - gameStartTime - gamePauseTime;
    }

    public void InitGame()
    {
        isPaused = false;
        gameTime = 0;
        gamePauseTime = 0;
        gameStartTime = Time.time;
        isGaming = true; 
    }

    TouchState CheckTouchState()
    {
        int tmpt = (int)(gameTime / 1.5f);
        if(tmpt % 3 == 0) return GameManager.TouchState.None;
        if(tmpt % 3 == 1) return GameManager.TouchState.Left;
        return TouchState.Right;
    }

    void CheckTouch()
    {
        nowTouchState = CheckTouchState();
        if(nowTouchState != prevTouchState)
        {
            if(nowTouchState == GameManager.TouchState.None)
            {
                myHandle.setRotate(Handle.RotateDir.Stop);
                mySubmarine.setMove(Submarine.MoveDir.Stop);
            }
            else if(nowTouchState == GameManager.TouchState.Left)
            {
                myHandle.setRotate(Handle.RotateDir.Left);
                mySubmarine.setMove(Submarine.MoveDir.Left);
            }
            else
            {
                myHandle.setRotate(Handle.RotateDir.Right);
                mySubmarine.setMove(Submarine.MoveDir.Right);
            }
            prevTouchState = nowTouchState;
        }
    }
}
