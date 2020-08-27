using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Number : MonoBehaviour
{
    private UnityEngine.UI.Image bg;
    private Text number_text;

    private Grid inGrid;//数字所在格子

    public NumberStatus status;


    private bool isPlayingSpawnAnim = false;
    private float spawnscaletime = 1;
    private bool isPlayingMergeAnim = false;
    private float mergescaletime = 1;
    private float backscaletime = 1;

    private float movePosTime = 1;
    private bool isMoving = false;
    private Vector3 startMove;
    private bool isDestroyOnMoveEnd = false;

    public Color[] bg_colors;
    public List<int> number_index;

    public AudioClip sound;

    private void Awake()
    {
        bg = transform.GetComponent<UnityEngine.UI.Image>();
        number_text = transform.Find("Text").GetComponent<Text>();
    }

    //初始化
    public void Init(Grid grid)
    {
        grid.SetNumber(this);

        this.SetGrid(grid);

        this.SetNumber(2);

        status = NumberStatus.Normal;

        PlaySpawnAnim();
    }

    //设置格子
    public void SetGrid(Grid grid)
    {
        this.inGrid = grid;
    }

    //获取格子
    public Grid GetGrid()
    {
        return this.inGrid;
    }

    //设置数字
    public void SetNumber(int number)
    {
        this.number_text.text = number.ToString();
        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
    }

    //获取数字
    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }

    //移动数字
    public void MoveToGrid(Grid grid)
    {
        transform.SetParent(grid.transform);
        //transform.localPosition = Vector3.zero;
        startMove = transform.localPosition;
        //endMove = grid.transform.position;

        movePosTime = 0;
        isMoving = true;

        this.GetGrid().SetNumber(null);

        grid.SetNumber(this);
        this.SetGrid(grid);
    }

    //合并
    public void Merge()
    {
        Game GamePanel = GameObject.Find("Canvas/Game_Panel").GetComponent<Game>();

        GamePanel.AddScore(this.GetNumber());
        int number = this.GetNumber() * 2;

        this.SetNumber(number);

        if (number == 2048)
        {
            //游戏胜利
            GamePanel.GameWin();
        }

        status = NumberStatus.NotMerge;

        PlayMergeAnim();

        AudioManager._instance.PlaySound(sound);
    }

    public bool IsMerge(Number number)
    {
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
            return true;
        return false;
    }

    //播放动画
    public void PlaySpawnAnim()
    {
        spawnscaletime = 0;
        isPlayingSpawnAnim = true;
    }

    public void PlayMergeAnim()
    {
        mergescaletime = 0;
        backscaletime = 0;
        isPlayingMergeAnim = true;
    }

    private void Update()
    {
        //创建动画
        if (isPlayingSpawnAnim)
        {
            if (spawnscaletime <= 1)
            {
                spawnscaletime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnscaletime);
            }
            else
            {
                isPlayingSpawnAnim = false;
            }
        }

        
        //合并动画

        if (isPlayingMergeAnim)
        {
            if (mergescaletime <= 1 && backscaletime == 0)
            {
                mergescaletime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergescaletime);
            }

            if (mergescaletime >= 1 && backscaletime <= 1)
            {
                backscaletime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, backscaletime);
            }
            
            if (mergescaletime >= 1 && backscaletime >= 1)
            {
                isPlayingMergeAnim = false;
            }
        }

        //移动动画
        if (isMoving)
        {
            
            if (movePosTime < 1)
            {
                movePosTime += Time.deltaTime * 5;
                transform.localPosition = Vector3.Lerp(startMove, Vector3.zero, movePosTime);
            }      
            else
            {
                isMoving = false;

                if(isDestroyOnMoveEnd)
                {
                    GameObject.Destroy(gameObject);
                }
            }

        }
    }

    public void MoveDestroyOnEnd(Grid grid)
    {
        transform.SetParent(grid.transform);
        startMove = transform.localPosition;
        //endMove = grid.transform.position;

        movePosTime = 0;
        isMoving = true;

        isDestroyOnMoveEnd = true;
    }


}
