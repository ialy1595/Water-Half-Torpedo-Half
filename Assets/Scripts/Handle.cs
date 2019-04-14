using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    public enum RotateDir
    {
        Left = 1,
        Stop = 0,
        Right = -1,
    };

    private const float handleRotateSpeed = 4.2f;

    private RotateDir rotateDir = 0;

    private float handleAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.myHandle = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.isPaused) return;
        handleAngle += (float)rotateDir * handleRotateSpeed;

        if(handleAngle > 360f) handleAngle -= 360f;
        if(handleAngle < -360f) handleAngle += 360f;

        transform.rotation = Quaternion.Euler(0f, 0f, handleAngle);
    }

    public void setRotate(RotateDir dir)
    {
        rotateDir = dir;
    }
}
