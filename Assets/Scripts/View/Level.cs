using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : View
{
    //点击设置游戏难度
    public void SelectClick(int count)
    {
        PlayerPrefs.SetInt(Const.GameModel, count);

        SceneManager.LoadSceneAsync(1);
    }
}
