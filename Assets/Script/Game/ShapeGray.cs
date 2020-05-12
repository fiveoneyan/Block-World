using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal.puremvc;
using UnityEngine;
using UnityEngine.UI;

public class ShapeGray : ComponentEx
{
    public Transform[] birthPviot = new Transform[3];//3个shape生成点；
    int PositionCount = 0;
    public static ShapeGray instance;
    public Button[] btnBoom;
    public int pp;
    public bool[] isClick = new bool[3];
    public bool[] isOver = new bool[3];
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        btnBoom[0].onClick.AddListener(OnBtnBoomClick01);
        btnBoom[1].onClick.AddListener(OnBtnBoomClick02);
        btnBoom[2].onClick.AddListener(OnBtnBoomClick03);

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override string[] ListNotificationInterests()
    {
        return new string[] {
            "MSG_ShapeGray",
            "MSG_GameRevive"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_ShapeGray":
                aaGray();
                break;
            case "MSG_GameRevive":
                aaGray();
                break;

        }
    }



    public void aaGray()
    {
        for (int i = 0; i < 3; i++)
        {
            if (birthPviot[i].childCount != 0)
            {

                if (CheckisGray(birthPviot[i].GetChild(0)) == 0)//如果需要变灰色
                {

                    BeGray(birthPviot[i].GetChild(0));
                    btnBoom[i].gameObject.SetActive(true);

                    isOver[i] = true;
                    //if (isClick[0])
                    //{
                    //    ChangeShape(0);
                    //    isClick[0] = false;
                    //}
                    //if (isClick[1])
                    //{
                    //    ChangeShape(1);
                    //    isClick[1] = false;
                    //}
                    //if (isClick[2])
                    //{
                    //    ChangeShape(2);
                    //    isClick[2] = false;
                    //}
                }
                else
                {
                    btnBoom[i].gameObject.SetActive(false);
                    isOver[i] = false;
                    ReCover(birthPviot[i].GetChild(0));

                }
            }
            else
            {
                isOver[i] = true;
            }

        }
        if (isOver[0] && isOver[1] && isOver[2])
        {
            if (PlayerPrefs.GetInt("isFirstTimes", 1) == 0)
            {
                //UIManager.Instance.OpenUI(EUITYPE.UIRevive);
                StartCoroutine(Waitpointfive());
            }
        }
    }

    IEnumerator Waitpointfive()
    {

        yield return new WaitForSeconds(1f);
        UIManager.Instance.OpenUI(EUITYPE.UIRevive);
        btnBoom[0].gameObject.SetActive(false);
        btnBoom[1].gameObject.SetActive(false);
        btnBoom[2].gameObject.SetActive(false);
        StopCoroutine(Waitpointfive());
    }

    void OnBtnBoomClick01()
    {

        // isClick[0]= true;
        ChangeShape(0);
        AudioManager.Instance.PlaySFX(Lib.instance.Gray_sound);
    }

    void OnBtnBoomClick02()
    {
        // isClick[1] = true;
        ChangeShape(1);
        AudioManager.Instance.PlaySFX(Lib.instance.Gray_sound);
    }
    void OnBtnBoomClick03()
    {
        AudioManager.Instance.PlaySFX(Lib.instance.Gray_sound);
        ChangeShape(2);
        // isClick[2] = true;
    }
    public void ChangeShape(int i)
    {
        Lib.instance.ShowRewardedVideo((boo) =>
        {
            if (boo)
            {
                btnBoom[i].gameObject.SetActive(false);
                birthPviot[i].GetChild(0).DOScale(0, 0.3f).OnComplete(() =>
                {
                    Destroy(birthPviot[i].GetChild(0).gameObject);
                    UIGame.instance.ShapePreview[i] = GameObject.Instantiate(Lib.instance.Shapes[0]).transform;
                    UIGame.instance.ShapePreview[i].SetParent(birthPviot[i]);
                    UIGame.instance.ShapePreview[i].localPosition = Vector3.zero;
                    UIGame.instance.ShapePreview[i].localScale = Vector3.zero;
                    UIGame.instance.ShapePreview[i].DOScale(0.4f, 0.3f);
                }); 
                
               
            }
        });

    }


    /// <summary>
    /// 检测是否需要变灰色
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    public int CheckisGray(Transform shape)
    {
        if (shape.GetComponent<Move>().destory)
        {
            return 10;
        }
        Vector3 NPos;//记录map中的空位置
        int count = 0;

        //Dictionary<int, Vector3> tempos = new Dictionary<int, Vector3>();
        //for (int i = 0; i < shape.childCount; i++)
        //{
        //    tempos[i] = shape.GetChild(i).position;
        //    tempos[i] = tempos[i]
        //}


        Dictionary<int, Vector3> RecordPosition = new Dictionary<int, Vector3>();// 记录唯一解的位置

        for (int i = 0; i < UIGame.instance.map.GetLength(1); i++)
        {
            for (int j = 0; j < UIGame.instance.map.GetLength(0); j++)
            {
                if (UIGame.instance.map[i, j] == null)
                {
                    NPos = SaveGround.instance.Groundmap[i, j].position;
                    Dictionary<int, Vector3> temporary = new Dictionary<int, Vector3>();//用一个临时位置储存shape孩子位置
                    shape.localScale = Vector3.one;
                    for (int a = 0; a < shape.childCount; a++)
                    {
                        temporary[a] = shape.GetChild(a).position;
                    }
                    float Xdistance, Ydistance;//shape孩子和空位置的距离
                    Xdistance = shape.GetChild(0).position.x - NPos.x;
                    Ydistance = shape.GetChild(0).position.y - NPos.y;
                    shape.localScale = Vector3.one * 0.4f;

                    for (int a = 0; a < shape.childCount; a++)//改变所有shape孩子的临时位置
                    {
                        temporary[a] = new Vector3(temporary[a].x - Xdistance, temporary[a].y - Ydistance, 0f);
                    }
                    if (PosLegal(temporary))
                    {
                        count++;
                    }

                }
            }

        }

        //if (count == 1)
        //{
        //    ShowPosition(RecordPosition);
        //}
        // UIGame.instance.isGameOver = true;
        //return false;
        //  Debug.Log(shape.gameObject.name+"+++++++"+count);
        return count;
    }

    /// <summary>
    /// 显示唯一解
    /// </summary>
    /// <param name="RecordPosition"></param>
    public void ShowPosition(Dictionary<int, Vector3> RecordPosition)
    {

        for (int i = 0; i < RecordPosition.Count; i++)
        {
            Vector3 childPos = RecordPosition[i];
            float cPosx, cPosy;
            cPosx = ((childPos.x + UIGame.instance.xPosx.x) / UIGame.instance.spacing);
            cPosy = ((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
            int xIndex = (int)Math.Round(cPosx);
            int yIndex = (int)Math.Round(cPosy);
            if (xIndex >= 7) { xIndex = 7; }
            if (yIndex >= 7) { yIndex = 7; }
            if (xIndex <= 0) { xIndex = 0; }
            if (yIndex <= 0) { yIndex = 0; }
            SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().sprite = Lib.instance.dicColor[0];

        }

    }

    /// <summary>
    /// 变成灰色
    /// </summary>
    /// <param name="shape"></param>
    public void BeGray(Transform shape)
    {
        // shape.localPosition = Vector3.zero;
        //shape.localScale = Vector3.one * 0.4f;
        for (int i = 0; i < shape.childCount; i++)
        {
            shape.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
    }

    /// <summary>
    /// 恢复颜色
    /// </summary>
    /// <param name="shape"></param>
    public void ReCover(Transform shape)
    {
        for (int i = 0; i < shape.childCount; i++)
        {
            shape.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }

    /// <summary>
    /// 检测位置是否合法
    /// </summary>
    public bool PosLegal(Dictionary<int, Vector3> temporary)
    {
        if (CheckisHum(temporary) && CheckisLap(temporary))
        {
            return true;
        }
        return false;
    }


    public bool CheckisHum(Dictionary<int, Vector3> temporary)
    {
        for (int i = 0; i < temporary.Count; i++)
        {
            Vector3 childPos = temporary[i];

            if (childPos.x < Aim.instance.MinPosx.position.x || childPos.x > Aim.instance.MaxPosx.position.x
                || childPos.y < Aim.instance.MinPosy.position.y || childPos.y > Aim.instance.MaxPosy.position.y)
            {
                return false;
            }

        }

        return true;
    }

    /// <summary>
    /// 是否重叠
    /// </summary>
    public bool CheckisLap(Dictionary<int, Vector3> temporary)
    {

        for (int i = 0; i < temporary.Count; i++)
        {
            Vector3 childPos = temporary[i];
            float cPosx, cPosy;
            cPosx = ((childPos.x + UIGame.instance.xPosx.x) / UIGame.instance.spacing);
            cPosy = ((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
            int xIndex = (int)Math.Round(cPosx);
            int yIndex = (int)Math.Round(cPosy);
            if (xIndex >= 7) { xIndex = 7; }
            if (yIndex >= 7) { yIndex = 7; }
            if (xIndex <= 0) { xIndex = 0; }
            if (yIndex <= 0) { yIndex = 0; }
            // Debug.Log("xxxxxxx"+xIndex+"yyyyyyy"+yIndex);
            if (UIGame.instance.map[xIndex, yIndex] != null)//有方块
            {
                return false;
            }

        }
        return true;
    }

}
