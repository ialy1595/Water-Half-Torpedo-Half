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

    private const float screenWidth = 720f;
    private const float screenHeight = 1280f;
    private const float stageWidth = 600f;
    private const float stageHeight = 900f;
    private const float moveTouchHeight = 1100f;
    private const int clearableMapWidth = 5;
    private const int clearableMapHeight = (int)(stageHeight / (
                                                    (Torpedo.minSpeed * stageWidth * 2f) / 
                                                    (clearableMapWidth * Submarine.submarineMoveSpeed)
                                                ));
    

    public static GameManager gm;

    public GameObject Gamemanager;
    public static GameObject _Gamemanager;
    private static bool GMCreated = false;

    [HideInInspector] public bool isGaming = false;
   
    [HideInInspector] public float gameTime;
    private float gameStartTime;
    
    [HideInInspector] public Handle myHandle;
    [HideInInspector] public Submarine mySubmarine;
    [HideInInspector] public Lazer myLazer;
    
    public GameObject torp;
    public GameObject myEndMessageText;

    private TouchState nowTouchState;
    private TouchState prevTouchState = 0;

    [HideInInspector] public int meter = 0;

    private int torpedoCreateCooltime = 100;


    void Awake()
    {
        if (GMCreated == true)  // GM 중복생성 방지
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
        Screen.SetResolution((int)screenWidth, (int)screenHeight, true);
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
                t.CheckDetect();
                isCrash = isCrash || t.CheckCrash();
            }
            if(isCrash) GameOver();
        }
    }

    public void InitGame()
    {
        Torpedo.TorpedoList.Clear();
        gameTime = 0;
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
            t.Detected(false);
        }
        Instantiate(myEndMessageText, new Vector3(0f, EndMessageText.posY, -2f), Quaternion.identity);
    }

    void TimeUpdate()
    {
        gameTime = Time.time - gameStartTime;
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
                if(pos.y < moveTouchHeight)
                {
                    if(pos.x < screenWidth / 2f) isLeft = true;
                    else isRight = true;
                }
            }
        }

        //for engine debug
        if(Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            if(pos.y < moveTouchHeight)
            {
                if(pos.x < screenWidth / 2f) isLeft = true;
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
        pos.y = Torpedo.createY;

        // for prevent stay
        // use continuous probability distribution

        float posx;
        float sp;

        while(true)
        {        
            bool reverseFlag = false;
            float tropCreateWidth = stageWidth - 2 * Torpedo.torpedoRadius;
            float p = Random.value;
            float t = (mySubmarine.getPos().x + (tropCreateWidth / 2f)) / tropCreateWidth;
            if(p > t)
            {
                t = 1 - t;
                p = 1 - p;
                reverseFlag = true;
            }

            float x = Mathf.Sqrt(t*p);
            if(reverseFlag) x = 1 - x;

            posx = x * tropCreateWidth - (tropCreateWidth / 2f);

            sp = Random.Range(Torpedo.minSpeed, Torpedo.maxSpeed);

            if(CheckClearable(posx, sp)) break;
        }

        pos.x = posx;

        instTorp = Instantiate(torp, pos, Quaternion.identity).GetComponent<Torpedo>();
        instTorp.torpedoMoveSpeed = sp;
    }

    int ClearableMapX(float x)
    {
        return (int)Mathf.Floor((x + (stageWidth / 2)) / (stageWidth / clearableMapWidth));
    }
    int ClearableMapY(float y, float speed)
    {
        Vector2 sPos = mySubmarine.getPos();
        float yf = ((stageWidth * 2f) / (clearableMapWidth * Submarine.submarineMoveSpeed));
        return (int)Mathf.Floor((y - sPos.y) / (speed * yf));
    }
    bool CheckClearable(float x, float sp)
    {
        Vector2 sPos = mySubmarine.getPos();
        Vector2 tPos;
        int i, j;
        int tx, ty;
        int sx = ClearableMapX(sPos.x);
        int[,] tMap = new int[10, 20];
        
        for(i=0;i<10;i++) for(j=0;j<20;j++) tMap[i,j] = 0;
        
        foreach (Torpedo t in Torpedo.TorpedoList)
        {
            tPos = t.getPos();
            tx = ClearableMapX(tPos.x);
            ty = ClearableMapY(tPos.y, t.torpedoMoveSpeed);
            if(ty < 0) ty = 0;
            if(ty > clearableMapHeight) ty = clearableMapHeight;
            tMap[tx, ty] = 1;
        }

        tMap[sx,0] = 2;
        for(j=1;j<=clearableMapHeight;j++) for(i=0;i<clearableMapWidth;i++) if(tMap[i,j] != 1 && tMap[i,j - 1] != 1)
        {
            if(
                (i > 0 && tMap[i - 1, j - 1] == 2) ||
                (tMap[i, j - 1] == 2) ||
                (i < clearableMapWidth - 1 && tMap[i + 1, j - 1] == 2)
            )
            {
                tMap[i,j] = 2;
            }
        }

        bool res = false;
        
        for(i=0;i<clearableMapWidth;i++) if(tMap[i, clearableMapHeight] == 2) res = true;
        
        // already gone
        if(!res) return true;
        
        tx = ClearableMapX(x);
        ty = ClearableMapY(Torpedo.createY, sp);
        if(ty < 0) ty = 0;
        if(ty > clearableMapHeight) ty = clearableMapHeight;
        tMap[tx, ty] = 1;

        tMap[sx,0] = 3;
        for(j=1;j<=clearableMapHeight;j++) for(i=0;i<clearableMapWidth;i++) if(tMap[i,j] != 1 && tMap[i,j - 1] != 1)
        {
            if(
                (i > 0 && tMap[i - 1, j - 1] == 3) ||
                (tMap[i, j - 1] == 3) ||
                (i < clearableMapWidth - 1 && tMap[i + 1, j - 1] == 3)
            )
            {
                tMap[i,j] = 3;
            }
        }

        res = false;
        
        for(i=0;i<clearableMapWidth;i++) if(tMap[i, clearableMapHeight] == 3) res = true;
        
        return res;
    }
}
