using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal.puremvc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShapeHint : ComponentEx, IPointerDownHandler, IPointerUpHandler
{
    public static ShapeHint instance;
    public bool isGray;
    public int hp;
    public int InColor;
    List<GameObject> TipsShape = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //hp = gameObject.GetComponent<ShapeColor>().ColorIndex;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
       

      //  OnlyHint(transform);

    }
    // Update is called once per frame
    void Update()
    {

    }
    public override string[] ListNotificationInterests()
    {
        return new string[] {
            "MSG_InColor",
             "MSG_ShowHint"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {

            case "MSG_InColor":
                InColor = (int)notification.body;
                break;
            case "MSG_ShowHint":
                // OnlyHint(transform);
                break;

        }
    }
    /// <summary>
    /// 唯一解检测
    /// </summary>
    /// <param name="shape"></param>
    public void OnlyHint(Transform shape)
    {
        Vector3 NPos;//记录map中的空位置
        int count = 0;

        Dictionary<int, Vector3> RecordPosition = new Dictionary<int, Vector3>();// 记录唯一解的位置
        //shape.localScale = Vector3.one;
        for (int i = 0; i < UIGame.instance.map.GetLength(1); i++)
        {
            for (int j = 0; j < UIGame.instance.map.GetLength(0); j++)
            {
                if (UIGame.instance.map[i, j] == null)
                {
                    NPos = SaveGround.instance.Groundmap[i, j].position;
                    Dictionary<int, Vector3> temporary = new Dictionary<int, Vector3>();//用一个临时位置储存shape孩子位置
                    for (int a = 0; a < shape.childCount; a++)
                    {
                        temporary[a] = shape.GetChild(a).position;
                    }
                    float Xdistance, Ydistance;//shape孩子和空位置的距离
                    Xdistance = shape.GetChild(0).position.x - NPos.x;
                    Ydistance = shape.GetChild(0).position.y - NPos.y;
                    for (int a = 0; a < shape.childCount; a++)//改变所有shape孩子的临时位置
                    {
                        temporary[a] = new Vector3(temporary[a].x - Xdistance, temporary[a].y - Ydistance, 0f);
                    }
                    if (PosLegal(temporary))
                    {
                        count++;
                        for (int a = 0; a < shape.childCount; a++)//记录唯一解的位置
                        {
                            RecordPosition[a] = temporary[a];
                        }
                    }

                }
            }

        }
        // shape.localScale = Vector3.one * 0.4f;
        if (count == 1)
        {
            ShowPosition(RecordPosition);
        }



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
            //SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().sprite = Lib.instance.dicColor[GetComponent<ShapeColor>().ColorIndex];
            //SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().color = Color.gray;
            //SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().DOFade(0.5f, 0);
            GameObject OnlyTips = PoolManager.instance.LoadObj(GType.Tips, transform.parent.parent);
            OnlyTips.transform.position = SaveGround.instance.Groundmap[xIndex, yIndex].position;
            OnlyTips.transform.SetAsFirstSibling();
            OnlyTips.GetComponent<Image>().sprite = Lib.instance.dicColor[GetComponent<ShapeColor>().ColorIndex];
            OnlyTips.GetComponent<Image>().color = Color.gray;
            TipsShape.Add(OnlyTips);
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
            if (UIGame.instance.map[xIndex, yIndex] != null)//有方块
            {
                return false;
            }

        }
        return true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 0; i < TipsShape.Count; i++)
        {
            PoolManager.instance.Add2Pool(GType.Tips, TipsShape[i]);
        }
    }
}
