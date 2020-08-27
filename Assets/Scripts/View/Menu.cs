using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public Level Level;
    public Set set;
    public AudioClip bg;

    private void Start()
    {
        AudioManager._instance.PlayerMusic(bg);
    }

    //点击开始游戏
    public void StartGameClick()
    {
        Level.Show();
    }

    //点击设置
    public void SetGameClick()
    {
        set.Show();
    }

    //点击退出游戏
    public void ExitGameClick()
    {
        Application.Quit();
    }
}
