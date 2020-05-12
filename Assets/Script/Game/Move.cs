using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;
using komal.puremvc;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using komal;

public class Move : ComponentEx, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    float Speed = 40f;
    float x, y;
    public bool isPress = false;
    public Vector2 NewPos;
    public int InColor;
    public static Move instance;
    public   List<Block> ShapeBlock = new List<Block>();
    public bool destory=false;
    public bool isThisShape;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }



    public void AddBlock(Block block )
    {
        if (!ShapeBlock.Contains(block))
        {
            ShapeBlock.Add(block);
        }
        if (ShapeBlock.Count == transform.childCount)
        {
            foreach (var i in ShapeBlock)
            {
                i.isChangeColor = true;
            }

        }
        else {
            foreach (var i in ShapeBlock)
            {
                i.isChangeColor = false;
            }
        }
       
    }

    public void RemoveBlock(Block block )
    {
        if (ShapeBlock.Contains(block))
        {
            ShapeBlock.Remove(block);
        }

            foreach (var i in ShapeBlock)
            {
                i.isChangeColor = false;
            }

    }



    public void OnPointerDown(PointerEventData eventData)
    {

        if (Lib.instance.move == null)

            Lib.instance.move = this;

        if (Lib.instance.move != this)
        {
           // this.transform.localScale = Vector3.one*0.4f;
            return;
        }
        if (PlayerPrefs.GetInt("Shake", 1) == 1)
        {
            KomalUtil.Instance.TapEngineSelection();

        }
        //print("按下！！！！");
        isPress = true;
        AudioManager.Instance.PlaySFX(Lib.instance.UP_sound);
        this.transform.localScale = Vector3.one;
        if (PlayerPrefs.GetInt("isFirstTimes", 1) == 1)
        {
            UIGame.instance.hand.gameObject.SetActive(false);
        }
       // transform.localPosition = new Vector3(0, 200, 0);
        transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 200, 0);
        this.GetComponent<ShapeHint>().OnlyHint(transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Lib.instance.move != this)
        {
            return;
        }

        transform.position = Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 200, 0);
        facade.SendNotification("MSG_RecoverColor");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Lib.instance.move == this)
        {
            Lib.instance.move = null;

            if (PlayerPrefs.GetInt("isFirstTimes", 1) == 1)
            {
                UIGame.instance.hand.gameObject.SetActive(true);
            }
            isPress = false;
            facade.SendNotification("MSG_DisShadow");
            if (!CheckisHum(transform))
            {
                this.transform.localPosition = Vector3.zero;
                this.transform.localScale = Vector3.one * 0.4f;
                AudioManager.Instance.PlaySFX(Lib.instance.Error_sound);
            }
            if (CheckisLap(transform) && ShapeBlock.Count == transform.childCount)
            {
                foreach (var i in ShapeBlock)
                {
                    i.MoveToPos();
                }
                AudioManager.Instance.PlaySFX(Lib.instance.Down_sound);
                if (PlayerPrefs.GetInt("Shake", 1) == 1)
                {
                    KomalUtil.Instance.TapEngineSelection();

                }
            }
            else
            {
                this.transform.localPosition = Vector3.zero;
                this.transform.localScale = Vector3.one * 0.4f;
                AudioManager.Instance.PlaySFX(Lib.instance.Error_sound);
            }
            SetPosition(this.transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPress && Lib.instance.move == this && CheckisHum(transform))
        {
            this.transform.localScale = Vector3.one;
        }

        //if (isPress && Lib.instance.move != this)
        //{
        //    this.transform.localScale = Vector3.one * 0.4f;
        //}


        //#if UNITY_EDITOR
        //        if (Input.GetMouseButton(0) && isPress)
        //#else
        //#endif

        //        // 单点触摸， 水平上下移动
        //        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && isPress)
        //        {

        //            //获取x轴
        //            x = Input.GetAxis("Mouse X") * Speed;
        //            //获取y轴
        //            y = Input.GetAxis("Mouse Y") * Speed;
        //            //this.transform.Translate(x, y, 0);
        //            //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //            //transform.Translate(touchDeltaPosition.x * Speed, touchDeltaPosition.y * Speed, 0);
        //            NewPos = transform.position;

        //        }

    }


    /// <summary>
    /// 储存方块
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector3 pos)
    {
        if (pos != Vector3.zero && !isPress && CheckisHum(this.transform))
        {
            destory = true;
            UIGame.instance.SaveShape(this.transform);

            Destroy(this);

            this.transform.SetParent(UIGame.instance.NewVersion);
            UIGame.instance.DisCount++;
            if (PlayerPrefs.GetInt("isFirstTimes", 1) == 1)
            {
                UIGame.instance.hand.gameObject.SetActive(false);
            }
            if (PlayerPrefs.GetInt("isFirstTimes", 1)==1 && UIGame.instance.aanew == 1)
            {
               
                UIGame.instance.aanew = 2;
                UIGame.instance.NewManLead();
                UIGame.instance.hand.gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("isFirstTimes", 1) == 1 && UIGame.instance.aanew == 2)
            {
               
                UIGame.instance.aanew = 3;
                UIGame.instance.NewManLead();
                UIGame.instance.hand.gameObject.SetActive(false);
            }
            else if (UIGame.instance.aanew == 3&& PlayerPrefs.GetInt("isFirstTimes", 1) == 1)
            {
                UIGame.instance.DelayToStart();
                UIGame.instance.hand.gameObject.SetActive(false);

             
            }
        }

        if (UIGame.instance.DisCount == 3)
        { 
            UIGame.instance.SpawnShape();
            UIGame.instance.DisCount = 0;
        }
        facade.SendNotification("MSG_ShapeGray");
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
