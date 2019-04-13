using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMessageText : MonoBehaviour
{
    // Start is called before the first frame update
    private Text endText;

    void Start()
    {
        endText = GetComponent<Text>();
        endText.text = "Bomb!!\nScore: " + GameManager.gm.meter + "m\nOK";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
