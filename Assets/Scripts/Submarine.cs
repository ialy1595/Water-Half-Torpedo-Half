using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    public enum MoveDir
    {
        Left = -1,
        Stop = 0,
        Right = 1,
    };

    [HideInInspector] public const float shortRadius = 40f;
    
    [HideInInspector] public const float submarineMoveSpeed = 6f;
    private const float edge = 260f;  

    private MoveDir moveDir = 0;

    private float submarineX = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.mySubmarine = this;
    }

    // Update is called once per frame
    void Update()
    {
        submarineX += (float)moveDir * submarineMoveSpeed;

        if(submarineX > edge) submarineX = edge;
        if(submarineX < -edge) submarineX = -edge;

        transform.position = (new Vector3(submarineX, transform.position.y, transform.position.z));
    }

    public void setMove(MoveDir dir)
    {
        moveDir = dir;
    }

    public Vector2 getPos()
    {
        return (new Vector2(transform.position.x, transform.position.y));
    }
}
