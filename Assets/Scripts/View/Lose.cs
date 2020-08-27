using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : View
{
    public void OnRestartClick()
    {
        SceneManager.LoadScene(1);
    }

    //退出游戏
    public void OnExitClick()
    {
        SceneManager.LoadScene(0);
    }
}
