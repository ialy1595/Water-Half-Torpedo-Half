using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCLickStart()
    {
        GameManager.gm.InitGame();
        SceneManager.LoadScene("Stage");
    }

    public void OnClickOK()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
