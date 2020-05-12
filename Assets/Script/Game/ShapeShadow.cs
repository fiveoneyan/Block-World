using System;
using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShapeShadow : ComponentEx
{
    public int InColor;
    public bool isPress = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        //print("按下！！！！");
        isPress = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //print("抬起！！！！");
        isPress = false;
        DisShadow();
        //if (CheckisHum(this.transform))
        //{

        Destroy(this);
        //}
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckisLap(transform) && CheckisHum(transform))
        {
            CheckShadow(transform);
        }

    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {
            "MSG_GetIndex",
            "MSG_DisShadow"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_GetIndex":
                InColor = (int)notification.body;
                break;
            case "MSG_DisShadow":
                DisShadow();
                break;
        }
    }
    /// <summary>
    /// 阴影
    /// </summary>
    /// <param name="shape"></param>
    public void CheckShadow(Transform shape)
    {
        if (CheckisPlace(this.transform))
        {
            DisShadow();
            for (int i = 0; i < shape.childCount; i++)
            {
                Vector3 childPos = shape.GetChild(i).position;
                float cPosx, cPosy;
                cPosx = ((childPos.x + UIGame.instance.xPosx.x) / UIGame.instance.spacing);
                cPosy = ((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
                int xIndex = (int)cPosx;
                int yIndex = (int)cPosy;
                if (xIndex >= 7) { xIndex = 7; }
                if (yIndex >= 7) { yIndex = 7; }
                if (xIndex <= 0) { xIndex = 0; }
                if (yIndex <= 0) { yIndex = 0; }
                if ((cPosx < xIndex + 0.3f && cPosy < yIndex + 0.3f))
                {
                    ShawShadow(xIndex, yIndex);
                }
                else if ((cPosx > xIndex + 0.7f && cPosy > yIndex + 0.7f))
                {
                    ShawShadow(xIndex + 1, yIndex + 1);
                }
                else if ((cPosx < xIndex + 0.3f && cPosy > yIndex + 0.3f))
                {
                    ShawShadow(xIndex, yIndex + 1);
                }
                else if ((cPosx > xIndex + 0.7f && cPosy < yIndex + 0.7f))
                {
                    ShawShadow(xIndex + 1, yIndex);
                }
                else
                {
                    DisShadow();
                }
            }

        }


    }


    /// <summary>
    /// 出现阴影
    /// </summary>
    public void ShawShadow(int xIndex, int yIndex)
    {
        if (xIndex >= 7) { xIndex = 7; }
        if (yIndex >= 7) { yIndex = 7; }
        if (xIndex <= 0) { xIndex = 0; }
        if (yIndex <= 0) { yIndex = 0; }
        if (CheckisPlace(this.transform))
        {
            SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().sprite = Lib.instance.dicColor[InColor];
            // SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().DOFade(0.3f, 0);
            SaveGround.instance.Groundmap[xIndex, yIndex].GetComponent<Image>().color = Color.gray;
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
                // SaveGround.instance.Groundmap[i, j].GetComponent<Image>().DOFade(0.5f, 0);
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


}
