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
        if(GameManager.gm.isPaused) return;
        GameManager.gm.mySoundEffect.SetSound(SoundEffect.Audio.Main);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCLickPause()
    {
        if(!GameManager.gm.isGaming) return;
        if(GameManager.gm.isPaused) return;
        GameManager.gm.isPaused = true;
        GameManager.gm.madePauseMessage = Instantiate(GameManager.gm.myPauseMessage, new Vector3(0f, GameManager.pauseMessageY, -2f), Quaternion.identity);
    }

    public void OnClickResume()
    {
        GameManager.gm.isPaused = false;
        Destroy(GameManager.gm.madePauseMessage);
    }

    public void OnClickExit()
    {
        GameManager.gm.mySoundEffect.SetSound(SoundEffect.Audio.Main);
        SceneManager.LoadScene("MainMenu");
    }
}
