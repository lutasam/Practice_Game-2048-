using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : View
{
    //重新开始
    public void OnRestartClick()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(1);
    }

    //退出游戏
    public void OnExitClick()
    {
        SceneManager.LoadScene(0);
    }
}
