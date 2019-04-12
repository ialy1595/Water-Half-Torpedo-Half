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
    [HideInInspector] public Lazer myLazer;

    public GameObject torp;
    
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

        Random.InitState((int)System.DateTime.Now.ToBinary());
    }

    void Update()
    {
        if(isGaming)
        {
            TimeUpdate();
            CheckTouch();
            CreateTorpedo();
            foreach (Torpedo t in Torpedo.TorpedoList)
            {
                t.checkDetect();
            }
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
        bool isLeft = false;
        bool isRight = false;        

        int count = Input.touchCount;
        if(count > 0)
        {
            for(int i=0;i<count;i++)
            {
                Vector2 pos = Input.GetTouch(i).position;
                if(pos.y < 400)
                {
                    if(pos.x < 0) isLeft = true;
                    else isRight = true;
                }
            }
        }

        //for engine debug
        if(Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            if(pos.y < 1100)
            {
                if(pos.x < 360) isLeft = true;
                else isRight = true;
            }
        }

        if(isLeft && (!isRight)) return TouchState.Left;
        if(isRight && (!isLeft)) return TouchState.Right;
        return TouchState.None;        
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

    private int torpedoCreateCooltime = 100;
    void CreateTorpedo()
    {
        if(torpedoCreateCooltime > 0)
        {
            torpedoCreateCooltime--;
            return;
        }

        torpedoCreateCooltime = 300;

        Torpedo instTorp;
        Vector2 pos;
        pos.y = 520;
        pos.x = Random.Range(-285f, 285f);

        instTorp = Instantiate(torp, pos, Quaternion.identity).GetComponent<Torpedo>();
        instTorp.torpedoMoveSpeed = Random.Range(0.5f, 3f);
    }
}
