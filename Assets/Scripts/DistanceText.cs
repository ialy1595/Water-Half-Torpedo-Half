using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Text dist;
    public float distPerTime = 10;
    
    void Start()
    {
        dist = GetComponent<Text>();
        dist.text = "0m";
    }

    // Update is called once per frame
    void Update()
    {
        dist.text = "" + (int)(GameManager.gm.gameTime * distPerTime) + "m";
    }
}
