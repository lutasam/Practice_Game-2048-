using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text text_score;//分数
    public Text text_best;//最高分
    public Button last;//上一步
    public Button restart;//重新开始
    public Button exit;//退出

    public Win winpanel;
    public Lose losepanel;

    public Transform gridParent;//格子父物体

    public Dictionary<int, int> gridConfig = new Dictionary<int, int>() { {4, 90}, { 5, 75}, { 6, 62} };

    public GameObject gridPrefab;

    public Grid[][] grids = null;

    private int row;//行数
    private int col;//列数

    public List<Grid> canCreateNumberGrid = new List<Grid>();

    public GameObject numberPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedCreateNumber = false;

    public int currentScore;

    public AudioClip bg;


    ////上一步
    //public void LastClick()
    //{

    //}

    //初始化格子
    public void InitGrid()
    {
        

        int gridNum = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        gridLayoutGroup.cellSize = new Vector2(gridConfig[gridNum], gridConfig[gridNum]);

        grids = new Grid[gridNum][];

        row = gridNum;
        col = gridNum;

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if (grids[i] == null)
                    grids[i] = new Grid[gridNum];
                grids[i][j] = CreateGrid();
            }
        }
    }

    //创建格子
    public Grid CreateGrid()
    {
        GameObject gameObject = GameObject.Instantiate(gridPrefab, gridParent);

        return gameObject.GetComponent<Grid>();
    }

    //创建数字
    public void CreateNumber()
    {
        canCreateNumberGrid.Clear();
        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    canCreateNumberGrid.Add(grids[i][j]);
                }
            }
        }
        if (canCreateNumberGrid.Count == 0)
            return;

        int index = Random.Range(0, canCreateNumberGrid.Count);

        GameObject gameObj = GameObject.Instantiate(numberPrefab, canCreateNumberGrid[index].transform);
        gameObj.GetComponent<Number>().Init(canCreateNumberGrid[index]);
    }

    private void Awake()
    {
        AudioManager._instance.PlayerMusic(bg);
        InitGrid();
        CreateNumber();
    }

    public void OnPointerDown()
    {
        pointerDownPos = Input.mousePosition;  
    }

    public void OnPointerUp()
    {
        pointerUpPos = Input.mousePosition;

        if (Vector3.Distance(pointerDownPos, pointerUpPos) < 100)
        {
            return;
        }

        MoveType movetype = CaculateMoveType();
        Debug.Log(movetype);
        MoveNumber(movetype);

        if(isNeedCreateNumber)
            CreateNumber();

        ResetNumberStatus();

        isNeedCreateNumber = false;

        if (IsGameLose())
        {
            GameLose();
        }
    }

    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerDownPos.y - pointerUpPos.y))
        {
            //左右移动
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {
                //右
                return MoveType.RIGHT;
            }
            else
            {
                //左
                return MoveType.LEFT;
            }
        }
        else
        {
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {
                //上
                return MoveType.TOP;
            }
            else
            {
                //下
                return MoveType.DOWN;
            }
        }
    }

    public void MoveNumber(MoveType movetype)
    {
        switch(movetype)
        {
            case MoveType.TOP:

                for (int j = 0; j < col; j++)
                {
                    for (int i = 1; i < row; i++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            for (int m = i - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }

                                HandleNumber(number, targetNumber, grids[m][j]);

                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                        
                    }
                }

                break;
            case MoveType.DOWN:

                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            for (int m = i + 1; m < row; m++)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }

                                HandleNumber(number, targetNumber, grids[m][j]);

                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }

                    }
                }

                break;
            case MoveType.LEFT:

                for (int i = 0; i < row; i++)
                {
                    for (int j = 1; j < col; j++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            for (int m = j - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }

                                HandleNumber(number, targetNumber, grids[i][m]);

                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.RIGHT:

                for (int i = 0; i < row; i++)
                {
                    for (int j = col - 2; j >= 0; j--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            for (int m = j + 1; m < col; m++)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }

                                HandleNumber(number, targetNumber, grids[i][m]);

                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                break;
        }
    }

    public void HandleNumber(Number current, Number target, Grid targetGrid)
    {
        if (target != null)
        {
            if (current.IsMerge(target))
            {
                target.Merge();

                current.GetGrid().SetNumber(null);
                //GameObject.Destroy(current.gameObject);
                current.MoveDestroyOnEnd(target.GetGrid());
                isNeedCreateNumber = true;
            }
        }
        else
        {
            current.MoveToGrid(targetGrid);
            isNeedCreateNumber = true;
        }
    }

    public void ResetNumberStatus()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].IsHaveNumber())
                {
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    //判断游戏是否失败
    public bool IsGameLose()
    {
        //判断格子是否满了
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    return false;
                }
            }
        }

        //判断有无数字能合并
        for (int i = 0; i < row; i += 2)
        {
            for (int j = 0; j < col; j++)
            {
                Grid up = IsHaveGrid(i - 1, j) ? grids[i - 1][j] : null;
                Grid down = IsHaveGrid(i + 1, j) ? grids[i + 1][j] : null;
                Grid left = IsHaveGrid(i, j - 1) ? grids[i][j - 1] : null;
                Grid right = IsHaveGrid(i, j + 1) ? grids[i][j + 1] : null;

                if (up != null)
                {
                    if(grids[i][j].GetNumber().IsMerge(up.GetNumber()))
                    {
                        return false;
                    }

                }

                if (down != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(down.GetNumber()))
                    {
                        return false;
                    }

                }

                if (left != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(left.GetNumber()))
                    {
                        return false;
                    }

                }

                if (right != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber()))
                    {
                        return false;
                    }

                }

            }
        }

        return true;
    }

    public bool IsHaveGrid(int i, int j)
    {

        if (i >= 0 && i < row && j >= 0 && j < col)
            return true;

        return false;
    }

    public void GameWin()
    {
        winpanel.Show();
    }

    public void GameLose()
    {
        losepanel.Show();
    }

    //public void RestartGame()
    //{
    //    //数据清空

    //    for (int i = 0; i < row; i++)
    //    {
    //        for (int j = 0; j < col; j++)
    //        {

    //            GameObject.Destroy(grids[i][j].GetNumber().gameObject);
    //            grids[i][j].SetNumber(null);

    //        }
    //    }
    //    //创建数字
    //    CreateNumber();
    //}

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore(currentScore);

        if (currentScore >= PlayerPrefs.GetInt(Const.BestScore, 0))
        {
            PlayerPrefs.SetInt(Const.BestScore, currentScore);
            UpdateBestScore(currentScore);
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScore(currentScore);
    }

    public void UpdateScore(int score)
    {
        this.text_score.text = score.ToString();
    }

    public void UpdateBestScore(int bestscore)
    {
        this.text_best.text = bestscore.ToString();
    }

}
