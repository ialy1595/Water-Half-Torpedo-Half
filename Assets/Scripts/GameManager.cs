using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
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
}
