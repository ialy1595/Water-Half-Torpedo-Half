using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    private float lazerRotateSpeed = 360f;

    private float lazerAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.myLazer = this;
    }

    // Update is called once per frame
    void Update()
    {
        lazerAngle = GameManager.gm.gameTime * lazerRotateSpeed;

        if(lazerAngle > 360f) lazerAngle -= 360f;
        if(lazerAngle < 0) lazerAngle += 360f;

        transform.rotation = Quaternion.Euler(0f, 0f, lazerAngle);
    }

    public float getAngle()
    {
        return lazerAngle;
    }
}
