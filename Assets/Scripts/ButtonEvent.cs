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
        GameManager.gm.mySoundEffect.SetSound(SoundEffect.Audio.Stage);
        SceneManager.LoadScene("Stage");
    }

    public void OnClickOK()
    {
        GameManager.gm.mySoundEffect.SetSound(SoundEffect.Audio.Main);
        SceneManager.LoadScene("MainMenu");
    }
}
