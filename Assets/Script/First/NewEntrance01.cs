using System;
using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;

public class NewEntrance01 : ComponentEx
{
    public static NewEntrance01 instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SaveShape(transform);
        // ChunkScript.GetComponent<UIGame>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveShape(Transform shape)
    {

        for (int i = 0; i < shape.childCount; i++)
        {
            Vector3 childPos = shape.GetChild(i).position;
            facade.SendNotification("MSG_DisShadow");
            int xIndex, yIndex;
            xIndex = (int)Math.Round((childPos.x + UIGame.instance. xPosx.x) / UIGame.instance.spacing);
            yIndex = (int)Math.Round((childPos.y + (-UIGame.instance.MPosy.y)) / UIGame.instance.spacing);
            
            if (xIndex >= 7) { xIndex = 7; }
            if (yIndex >= 7) { yIndex = 7; }
            if (xIndex <= 0) { xIndex = 0; }
            if (yIndex <= 0) { yIndex = 0; }
            UIGame.instance.map[xIndex, yIndex] = shape.GetChild(i);
            

        }

       UIGame.instance.CheckClear();
       // ChunkScript.GetComponent<UIGame>().enabled = true;
      //  UIGame.instance.StartGame();

    }

   
}
