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
    
    public GameObject myEndMessageText;

    private TouchState nowTouchState;
    private TouchState prevTouchState = 0;

    [HideInInspector] public int meter = 0;
    

    void Awake()
    {
        if (GMCreated == true)  // GM 중복생성 방지
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
        Screen.SetResolution(720, 1280, true);
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

            bool isCrash = false;
            foreach (Torpedo t in Torpedo.TorpedoList)
            {
                t.checkDetect();
                isCrash = isCrash || t.checkCrash();
            }
            if(isCrash) GameOver();
        }
    }

    public void InitGame()
    {
        Torpedo.TorpedoList.Clear();
        isPaused = false;
        gameTime = 0;
        gamePauseTime = 0;
        gameStartTime = Time.time;
        meter = 0;
        isGaming = true; 
    }

    void GameOver()
    {
        isGaming = false;
        myHandle.setRotate(Handle.RotateDir.Stop);
        mySubmarine.setMove(Submarine.MoveDir.Stop);
        foreach (Torpedo t in Torpedo.TorpedoList)
        {
            t.detected(false);
        }
        Instantiate(myEndMessageText, new Vector3(0f, 100f, -2f), Quaternion.identity);
    }

    void TimeUpdate()
    {
        gameTime = Time.time - gameStartTime - gamePauseTime;
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
                if(pos.y < 1100)
                {
                    if(pos.x < 360) isLeft = true;
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

        torpedoCreateCooltime = Random.Range(0, 80);

        Torpedo instTorp;
        Vector2 pos;
        pos.y = 520;

        // for prevent stay
        // use continuous probability distribution

        float posx;
        float sp;

        while(true)
        {        
            bool reverseFlag = false;
            float p = Random.value;
            float t = (mySubmarine.getPos().x + 285f) / 570f;
            if(p > t)
            {
                t = 1 - t;
                p = 1 - p;
                reverseFlag = true;
            }

            float x = Mathf.Sqrt(t*p);
            if(reverseFlag) x = 1 - x;

            posx = x * 570f - 285f;

            sp = Random.Range(Torpedo.minSpeed, Torpedo.maxSpeed);

            if(CheckClearable(posx, sp)) break;
        }

        pos.x = posx;

        instTorp = Instantiate(torp, pos, Quaternion.identity).GetComponent<Torpedo>();
        instTorp.torpedoMoveSpeed = sp;
    }

    bool CheckClearable(float x, float sp)
    {
        Vector2 sPos = mySubmarine.getPos();
        Vector2 tPos;
        int i, j;
        int tx, ty;
        int sx = (int)Mathf.Floor((sPos.x + 300f) / (600f / 5f));
        int[,] tMap = new int[10, 20];
        
        for(i=0;i<10;i++) for(j=0;j<20;j++) tMap[i,j] = 0;
        
        foreach (Torpedo t in Torpedo.TorpedoList)
        {
            tPos = t.getPos();
            tx = (int)Mathf.Floor((tPos.x + 300f) / (600f / 5f));
            ty = (int)Mathf.Floor((tPos.y - sPos.y) / (t.torpedoMoveSpeed * 40f));
            if(ty < 0) ty = 0;
            if(ty > 9) ty = 9;
            tMap[tx, ty] = 1;
        }

        tMap[sx,0] = 2;
        for(j=1;j<10;j++) for(i=0;i<5;i++) if(tMap[i,j] != 1 && tMap[i,j - 1] != 1)
        {
            if(
                (i > 0 && tMap[i - 1, j - 1] == 2) ||
                (tMap[i, j - 1] == 2) ||
                (i < 4 && tMap[i + 1, j - 1] == 2)
            )
            {
                tMap[i,j] = 2;
            }
        }

        bool res = false;
        
        for(i=0;i<5;i++) if(tMap[i, 9] == 2) res = true;
        
        // already gone
        if(!res) return true;
        
        tx = (int)Mathf.Floor((x + 300f) / (600f / 5f));
        ty = (int)Mathf.Floor((520 - sPos.y) / (sp * 40f));
        if(ty < 0) ty = 0;
        if(ty > 9) ty = 9;
        tMap[tx, ty] = 1;

        tMap[sx,0] = 3;
        for(j=1;j<10;j++) for(i=0;i<5;i++) if(tMap[i,j] != 1 && tMap[i,j - 1] != 1)
        {
            if(
                (i > 0 && tMap[i - 1, j - 1] == 3) ||
                (tMap[i, j - 1] == 3) ||
                (i < 4 && tMap[i + 1, j - 1] == 3)
            )
            {
                tMap[i,j] = 3;
            }
        }

        res = false;
        
        for(i=0;i<5;i++) if(tMap[i, 9] == 3) res = true;
        
        return res;
    }
}
