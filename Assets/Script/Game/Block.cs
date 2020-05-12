using System;
using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using UnityEngine.UI;

public class Block : ComponentEx
{

    GameObject chunk;
   public bool isChangeColor;
    //public override string[] ListNotificationInterests()
    //{
    //    return new string[] {
    //       "MSG_DisFour"
    //        };
    //}

    //public override void HandleNotification(INotification notification)
    //{
    //    switch (notification.name)
    //    {
    //        case "MSG_DisFour":
    //             StartCoroutine(ClearReicol());
    //            break;

    //    }
    //}
    void Start()
    {
        gameObject.tag = "Block";
        BoxCollider2D bc2d = gameObject.AddComponent<BoxCollider2D>();
        bc2d.size = new Vector2(96, 96);
        gameObject.layer = 8;
    }
    RaycastHit2D hit;
    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, -Vector3.forward,20,~(1<<8));
        if (hit && hit.collider.tag == "Chunk")
        {
            if (chunk != null&&chunk != hit.collider.gameObject)
            {
                transform.parent.GetComponent<Move>().RemoveBlock(this);
                chunk.GetComponent<Image>().sprite = Lib.instance.Ground;
                chunk.GetComponent<Image>().color = Color.white;
            }
            chunk = hit.collider.gameObject;
            transform.parent.GetComponent<Move>().AddBlock(this);
            if (isChangeColor)
            {
                chunk.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                chunk.GetComponent<Image>().color = Color.gray;
            }
            else
            {

                chunk.GetComponent<Image>().sprite = Lib.instance.Ground;
                chunk.GetComponent<Image>().color = Color.white;
            }
        }
        else {
            if (chunk != null)
            {
                transform.parent.GetComponent<Move>().RemoveBlock(this);
                chunk.GetComponent<Image>().sprite = Lib.instance.Ground;
                chunk.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void MoveToPos()
    {
        transform.position = chunk.transform.position+new Vector3(0,0,-1);
        chunk.GetComponent<Image>().sprite = Lib.instance.Ground;
        chunk.GetComponent<Image>().color = Color.white;
        gameObject.layer = 5;
        this.enabled = false;

    }
    public void SaveOriginalPos()
    {
            Vector3 childPos = chunk.transform.position;
            float cPosx, cPosy;
            cPosx = ((childPos.x + UIGame.instance.xPosx.x) / UIGame.instance.spacing);
            cPosy = ((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
            int xIndex = (int)Math.Round(cPosx);
            int yIndex = (int)Math.Round(cPosy);
            if (xIndex >= 7) { xIndex = 7; }
            if (yIndex >= 7) { yIndex = 7; }
            if (xIndex <= 0) { xIndex = 0; }
            if (yIndex <= 0) { yIndex = 0; }
            if (UIGame.instance.Originalmap[xIndex, yIndex] == null)
            {
                UIGame.instance.Originalmap[xIndex, yIndex] = transform;
            }
        }

   

}
