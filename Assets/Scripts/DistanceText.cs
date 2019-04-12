using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Text dist;
    public float distPerTime = 10;
    [HideInInspector] public int meter;

    void Start()
    {
        dist = GetComponent<Text>();
        dist.text = "0m";
    }

    // Update is called once per frame
    void Update()
    {
        meter = (int)(GameManager.gm.gameTime * distPerTime);
        string meterString = ""+meter;
        if(meter < 10000) meterString = "0" + meterString;
        if(meter < 1000) meterString = "0" + meterString;
        if(meter < 100) meterString = "0" + meterString;
        if(meter < 10) meterString = "0" + meterString;
        dist.text = "" + meterString + "m";
    }
}
