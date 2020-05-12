using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal;
using komal.puremvc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShapeColor : ComponentEx, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int ColorIndex;
    public bool isPress = false;
    //float x, y;
    //float Speed = 50;
    public  int aaColor;
    public void OnPointerDown(PointerEventData eventData)
    {
        //print("按下！！！！");
        isPress = true;


    }
    public void OnDrag(PointerEventData eventData)
    {
        facade.SendNotification("MSG_RecoverColor");
        if (CheckisLap(transform) && CheckisHum(transform))
        {
            CheckColor(transform);
        }
       


    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //print("抬起！！！！");
        isPress = false;
        //facade.SendNotification("MSG_RecoverColor");

    }
    // Start is called before the first frame update
    void Start()
    {
       
        ColorIndex = UnityEngine.Random.Range(0, Lib.instance.imgColor.Length);
        for (int j = 0; j < transform.childCount; j++)
        {
            //  Debug.Log("bbbbbbb"+(ShapePool[i].childCount-1));
            transform.GetChild(j).GetComponent<Image>().sprite = Lib.instance.dicColor[ColorIndex];
        }

    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {
            "MSG_DisShadow",
           "MSG_RecoverColor",
           "MSG_DisFour"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {

            case "MSG_DisShadow":
                DisShadow();
                break;
            case "MSG_RecoverColor":
                RecoverColor();
                break;
            case "MSG_DisFour":
                 StartCoroutine(ClearReicol());
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isPress)
        //{ return; }
        // 单点触摸， 水平上下移动
        // if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && isPress)
        //{
        //    //获取x轴
        //    x = Input.GetAxis("Mouse X") * Speed;
        //    //获取y轴
        //    y = Input.GetAxis("Mouse Y") * Speed;

        //}




    }

    GType GetType()
    {
        
        GType type = GType.BLUE;
        switch (aaColor)
        {
            case 0:
                type = GType.BLUE;
                break;
            case 1:
                type = GType.GREEN;
                break;
            case 2:
                type = GType.PURPLE;
                break;
            case 3:
                type = GType.PINK;
                break;
            case 4:
                type = GType.RED;
                break;
            case 5:
                type = GType.YELLOW;
                break;
        }
        return type;
    }
    IEnumerator ClearReicol()
    {
        for (int j = 3; j >= 0; j--)
        {
            for (int i = 0; i < UIGame.instance.Originalmap02.GetLength(1); i++)
            {
                if (UIGame.instance.Originalmap02[i, j] != null)
                {
                    Transform block = UIGame.instance.Originalmap02[i, j];
                    aaColor = block.parent.GetComponent<ShapeColor>().ColorIndex;
                    UIGame.instance.Originalmap02[i, j] = null;
                    GameObject GameEffect = PoolManager.instance.LoadObj(GetType(), transform);
                    GameEffect.transform.position = block.position;
                    PoolManager.instance.Delay2Pool(GetType(), GameEffect, 5);
                    DestroyImmediate(block.gameObject);

                }
                yield return new WaitForSeconds(0.03f);

            }
        }


    }

    /// <summary>
    /// 是否到达边界
    /// </summary>
    public bool CheckisHum(Transform shape)
    {
        for (int i = 0; i < shape.childCount; i++)
        {
            Vector3 childPos = shape.GetChild(i).position;

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
    public bool CheckisLap(Transform shape)
    {
        if (CheckisHum(shape))
        {
            for (int i = 0; i < shape.childCount; i++)
            {
                Vector3 childPos = shape.GetChild(i).position;
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

        }

        return true;
    }

    /// <summary>
    /// 位置是否合理
    /// </summary>
    /// <returns></returns>
    public bool CheckisPlace(Transform shape)
    {

        if (CheckisHum(this.transform) && CheckisLap(this.transform))
        {

            return true;
        }
        return false;
    }



    /// <summary>
    /// 检测变色
    /// </summary>
    /// <param name="shape"></param>
    public void CheckColor(Transform shape)
    {

        UIGame.instance.AssignMap();
       
        if (CheckisPlace(this.transform) && transform.GetComponent<Move>().ShapeBlock.Count == transform.childCount)
        {
            
                foreach (var i in transform.GetComponent<Move>().ShapeBlock)
                {
                    i.SaveOriginalPos();
                }

            //for (int i = 0; i < shape.childCount; i++)
            //{

            //    Vector3 childPos = shape.GetChild(i).position;
            //    float cPosx, cPosy;
            //    cPosx = ((childPos.x + UIGame.instance.xPosx.x) / UIGame.instance.spacing);
            //    cPosy = ((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
            //    int xIndex = (int)Math.Round(cPosx);
            //    int yIndex = (int)Math.Round(cPosy);
            //    if (xIndex >= 7) { xIndex = 7; }
            //    if (yIndex >= 7) { yIndex = 7; }
            //    if (xIndex <= 0) { xIndex = 0; }
            //    if (yIndex <= 0) { yIndex = 0; }
            //    if (UIGame.instance.Originalmap[xIndex, yIndex] == null)
            //    {
            //        UIGame.instance.Originalmap[xIndex, yIndex] = shape.GetChild(i);
            //    }
            //}
            ChangeColor();
        }
        //facade.SendNotification("MSG_RecoverColor");
    }

    /// <summary>
    /// 改变行/列颜色
    /// </summary>
    public void ChangeColor()
    {
        facade.SendNotification("MSG_SetColorIndex", ColorIndex);

        List<int> rowIndex = new List<int>();
        List<int> colIndex = new List<int>();
        for (int i = 0; i < UIGame.instance.Originalmap.GetLength(1); i++)
        {
            bool isClearRow = true;
            for (int j = 0; j < UIGame.instance.Originalmap.GetLength(0); j++)
            {
                if (UIGame.instance.Originalmap[j, i] == null)
                {
                    isClearRow = false;
                    break;
                }
            }
            if (isClearRow)
            {
                rowIndex.Add(i);

            }

        }
        for (int a = 0; a < rowIndex.Count; a++)
        {

            for (int b = 0; b < UIGame.instance.Originalmap.GetLength(0); b++)
            {
                if (UIGame.instance.Originalmap[b, rowIndex[a]] != null)
                {
                    Transform Block = UIGame.instance.Originalmap[b, rowIndex[a]];
                    Block.GetComponent<Image>().sprite = Lib.instance.dicColor[ColorIndex];
                }
            }
        }

        for (int i = 0; i < UIGame.instance.Originalmap.GetLength(0); i++)
        {
            bool isClearCol = true;
            for (int j = 0; j < UIGame.instance.Originalmap.GetLength(1); j++)
            {

                if (UIGame.instance.Originalmap[i, j] == null)
                {
                    isClearCol = false;
                    break;
                }
            }
            if (isClearCol)
            {
                colIndex.Add(i);
            }
        }
        for (int a = 0; a < colIndex.Count; a++)
        {
            for (int b = 0; b < UIGame.instance.Originalmap.GetLength(0); b++)
            {
                if (UIGame.instance.Originalmap[colIndex[a], b] != null)
                {
                    Transform Block = UIGame.instance.Originalmap[colIndex[a], b];
                    Block.GetComponent<Image>().sprite = Lib.instance.dicColor[ColorIndex];
                }
            }
        }
    }

    /// <summary>
    /// 恢复原来颜色
    /// </summary>
    /// <param name="pos"></param>
    public void RecoverColor()
    {

        UIGame.instance.AssignMap();
        for (int i = 0; i < UIGame.instance.Originalmap.GetLength(1); i++)
        {
            bool isClearRow = true;
            for (int j = 0; j < UIGame.instance.Originalmap.GetLength(0); j++)
            {
                if (UIGame.instance.map[j, i] == null)
                {
                    isClearRow = false;
                    break;
                }
            }
            if (!isClearRow)
            {
                for (int j = 0; j < transform.childCount; j++)
                {

                    transform.GetChild(j).GetComponent<Image>().sprite = Lib.instance.dicColor[ColorIndex];
                }
            }
        }

        for (int i = 0; i < UIGame.instance.Originalmap.GetLength(0); i++)
        {
            bool isClearCol = true;
            for (int j = 0; j < UIGame.instance.Originalmap.GetLength(1); j++)
            {

                if (UIGame.instance.map[i, j] == null)
                {
                    isClearCol = false;
                    break;
                }
            }
            if (!isClearCol)
            {
                for (int j = 0; j < transform.childCount; j++)
                {

                    transform.GetChild(j).GetComponent<Image>().sprite = Lib.instance.dicColor[ColorIndex];

                }
            }
        }
    }









    /// <summary>
    /// 阴影消失
    /// </summary>
    public void DisShadow()
    {
        for (int i = 0; i < SaveGround.instance.Groundmap.GetLength(1); i++)
        {

            for (int j = 0; j < SaveGround.instance.Groundmap.GetLength(0); j++)
            {
                SaveGround.instance.Groundmap[i, j].GetComponent<Image>().sprite = Lib.instance.Ground;
                SaveGround.instance.Groundmap[i, j].GetComponent<Image>().color = Color.white;

            }


        }

    }





}
