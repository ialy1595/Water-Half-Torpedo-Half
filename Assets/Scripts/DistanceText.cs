using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    // Start is called before the first frame update
    
    public const float distPerTime = 10;
    
    private Text dist;
    [HideInInspector] public int meter;

    void Start()
    {
        dist = GetComponent<Text>();
        dist.text = "0m";
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.isPaused) return;
        if(GameManager.gm.isGaming)
        {
            meter = (int)(GameManager.gm.gameTime * distPerTime);
            string meterString = ""+meter;
            if(meter < 10000) meterString = "0" + meterString;
            if(meter < 1000) meterString = "0" + meterString;
            if(meter < 100) meterString = "0" + meterString;
            if(meter < 10) meterString = "0" + meterString;
            dist.text = "" + meterString + "m";
            GameManager.gm.meter = meter;
        }
        else
        {
            dist.text = "BOMB!!";
        }
    }
}
