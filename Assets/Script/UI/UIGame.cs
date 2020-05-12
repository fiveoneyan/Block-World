using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using komal.sdk;
using System.Collections.Generic;
using System.Collections;
using komal.puremvc;
using komal;

public class UIGame : UIBase
{
    public Transform transBg;
    public Button btnSetting;
    public Text txtBest;
    public Text txtScore;
    public float spacing;//两个方块间的间距
    public int xIndex, yIndex;
    GameState gameState = GameState.Stop;
    public RectTransform MaxPosx;
    public RectTransform MinPosx;
    public RectTransform MaxPosy;
    public RectTransform MinPosy;
    public Vector3 xPosx;
    public Vector3 MPosx;
    public Vector3 xPosy;
    public Vector3 MPosy;
    public Transform[,] map = new Transform[8, 8];//储存方块信息
    public Transform[,] Originalmap = new Transform[8, 8];//原始方块信息
    public Transform[,] Originalmap02 = new Transform[8, 8];//原始方块信息
    public Transform currentShape;//当前形状
    public Transform[] ShapePreview = new Transform[3];//预览Shape
    public Transform[] birthPviot = new Transform[3];//3个shape生成点；
    public Transform NewVersion;
    public int Score = 0;//积分
    public int bestScore;//最高积分
    public static UIGame instance;
    int[] ShapeIndex = new int[3];
    public int DisCount = 0;//shape是否满三
    public int[] rand01 = new int[31] { 4, 3, 3, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 4, 4, 4, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 6 };
    public int[] rand02 = new int[31] { 4, 3, 3, 4, 4, 3, 3, 3, 3, 4, 4, 4, 4, 4, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 8 };
    public int[] rand03 = new int[31] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 4, 4, 3, 3, 3, 3, 10 };
    public int[] rand04 = new int[31] { 3, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 3, 3, 3, 3, 12 };
    public Transform hand;
    public int Revivetimes=0;
    public int ColorIndex;
    public bool isFirstTimes=true;
    public int aanew=1;
    #region LifeTime
    public override void OnEnter()
    {
        transform.localScale = Vector3.one * 1.05f;
        txtBest.text = PlayerPrefs.GetInt("bestScore", 0).ToString();
        AudioManager.Instance.PlayBGM(Lib.instance.Bg_sound);
        base.OnEnter();
        instance = this;
        transBg.DOScale(1, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Time.timeScale = 0;
        });

        btnSetting.onClick.AddListener(OnBtnSettingClick);
        if (PlayerPrefs.GetInt("isFirstTimes", 1) == 1)
        {
            StartCoroutine(WaitHand());
            NewManLead();
        }
        else { StartGame(); }
        
        
        // StartGame();

    }

    IEnumerator WaitHand()
    {
        yield return new WaitForSeconds(1f);
        hand.gameObject.SetActive(true);
    }
    public override void OnResume()
    {
        base.OnResume();
        xPosx = MaxPosx.position;
        MPosx = MinPosx.position;
        xPosy = MaxPosy.position;
        MPosy = MinPosy.position;
        spacing = Mathf.Abs((xPosx.x - MPosx.x) / 7);
        
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnExit()
    {
        base.OnExit();
        btnSetting.onClick.RemoveListener(OnBtnSettingClick);
    }
    #endregion
    void Update()
    {
        
    }

    public void NewManLead()
    {

        StartCoroutine(BirthNewShape(aanew)); 
    }

    IEnumerator BirthNewShape(int aanew)
    {
        switch (aanew)
        {
            case 1:
                yield return new WaitForSeconds(1f);
                GameObject NewShape01 = PoolManager.instance.LoadObj(GType.NewShape01, NewVersion);
                NewShape01.transform.localPosition = new Vector3(0, -114, 0);
                //NewShape01.transform.DOScale(new Vector3(0, 0, 0), 0f);
                //NewShape01.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                NewBirth01();
                yield return new WaitForSeconds(0.8f);
                hand.gameObject.SetActive(true);
                break;
            case 2:
                yield return new WaitForSeconds(2f);
                GameObject NewShape02 = PoolManager.instance.LoadObj(GType.NewShape02, NewVersion);
                NewShape02.transform.localPosition = new Vector3(0, -111, 0);
                //NewShape02.transform.DOScale(new Vector3(0, 0, 0), 0f);
                //NewShape02.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                NewBirth02();
                yield return new WaitForSeconds(0.8f);
                hand.gameObject.SetActive(true);
                break;
            case 3:

                yield return new WaitForSeconds(2f);
                GameObject NewShape03 = PoolManager.instance.LoadObj(GType.NewShape03, NewVersion);
                NewShape03.transform.localPosition = new Vector3(0, 0, 0);
                //NewShape03.transform.DOScale(new Vector3(0, 0, 0), 0f);
                //NewShape03.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                NewBirth03();
                yield return new WaitForSeconds(0.8f);
                hand.gameObject.SetActive(true);
                break;

        }

    }
    void Init(params object[] param)
    {

    }
    public override string[] ListNotificationInterests()
    {
        return new string[] {
            "MSG_GameOver",
            "MSG_GameStart",
            "MSG_GameRevive",
            "MSG_GameBeGray",
            "MSG_SetColorIndex"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_GameOver":
                StopGame();
                break;
            case "MSG_GameStart":
                ReGame();
                StartGame();
                facade.SendNotification("MSG_ShapeGray");
                break;
            case "MSG_GameRevive":
                ReviveGame();
                break;
            case "MSG_GameBeGray":
                ShapeGray.instance.isOver[0] = false;
                ShapeGray.instance.isOver[1] = false;
                ShapeGray.instance.isOver[2] = false;
                StartCoroutine(BeGrayaa());
                StartCoroutine(waitasecond());
                break;
            case "MSG_SetColorIndex":
                ColorIndex = (int)notification.body;
                break;

        }
    }

   

    IEnumerator waitasecond()
    {
        yield return new WaitForSeconds(2);
        if (PlayerPrefs.GetInt("bestScore", 0) < PlayerPrefs.GetInt("Score", 0))
        {
            UIManager.Instance.OpenUI(EUITYPE.UIVictory);
            BestScore(PlayerPrefs.GetInt("bestScore", 0), Score);
        }
        else
        {
            BestScore(PlayerPrefs.GetInt("bestScore", 0), Score);
            UIManager.Instance.OpenUI(EUITYPE.UIDefeat);
        }
        StopCoroutine(waitasecond());
    }
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ReGame()
    {
        // 重置游戏

        for (int j = 0; j < NewVersion.childCount; j++)
        {
            Destroy(NewVersion.GetChild(j).gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ShapePreview[i].childCount; j++)
            {
                Destroy(ShapePreview[i].GetChild(j).gameObject);
                //ShapePreview[i] = null;
            }

            if (birthPviot[i].childCount != 0)
            {
                for (int j = 0; j < birthPviot[i].childCount; j++)
                {
                    Destroy(birthPviot[i].GetChild(0).gameObject);
                    //birthPviot[i] = null;
                }
            }
        }
       
    }
    /// <summary>
    /// 结束变灰
    /// </summary>
    /// <returns></returns>
    IEnumerator BeGrayaa()
    {
        AudioManager.Instance.PlaySFX(Lib.instance.Fail_sound);
      //  facade.SendNotification("MSG_ShapeGray");
        AssignMap();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = null;

            }
        }
        float a = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                a = a + 0.03f;
                if (Originalmap[i, j] != null)
                {
                    Originalmap[i, j].GetComponent<Image>().DOFade(0.5f, a);
                }

            }
        }
        yield return new WaitForSeconds(1);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    Destroy(Originalmap[i, j].gameObject);
                }
                Originalmap[i, j] = null;

            }
        }
       
        StopCoroutine(BeGrayaa());


    }
    /// <summary>
    /// 重新开始时重置
    /// </summary>
    public void ReStartClear()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    Destroy(map[i, j].gameObject);
                }
                map[i, j] = null;

            }
        }
    }

    public void DelayToStart()
    {
        Invoke("StartGame",1f);
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("isFirstTimes", 1) != 0)
        {
            PlayerPrefs.SetInt("isFirstTimes", 0);
            for (int j = 0; j < NewVersion.childCount; j++)
            {
                Destroy(NewVersion.GetChild(j).gameObject);
            }
        }
        PlayerPrefs.SetInt("play_times", PlayerPrefs.GetInt("play_times", 1) + 1);
        if (gameState == GameState.Stop)
        {
            DisCount = 0;
            Score = 0;
            txtScore.text = "0";
            gameState = GameState.Start;
            SpawnShape();
            txtBest.text = PlayerPrefs.GetInt("bestScore", 0).ToString();
            PlayerPrefs.SetInt("Score", 0);
        }
        
        
    }
    /// <summary>
    /// 结束游戏
    /// </summary>
    public void StopGame()
    {

        if (gameState == GameState.Start)
        {
            gameState = GameState.Stop;
        }
       
       
       
        ShapeGray.instance.isOver[0] = false;
        ShapeGray.instance.isOver[1] = false;
        ShapeGray.instance.isOver[2] = false;
        
    }
    /// <summary>
    /// 复活游戏
    /// </summary>
    public void ReviveGame()
    {
        ShapeGray.instance.isOver[0] = false;
        ShapeGray.instance.isOver[1] = false;
        ShapeGray.instance.isOver[2] = false;
        AssignMap02();
        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (map[i, j] != null)
                {

                    map[i, j] = null;
                }
            }
        }
        facade.SendNotification("MSG_ShapeGray");

        //StartCoroutine(ClearReicol());
        facade.SendNotification("MSG_DisFour");
        //for (int i = 0; i < map.GetLength(1); i++)
        //{
        //    for (int j = 0; j < 4; j++)
        //    {
        //        if (map[i, j] != null)
        //        {
        //            GameObject GameEffect = PoolManager.instance.LoadObj(GetType(), transform);
        //            GameEffect.transform.position = map[i, j].position;
        //            PoolManager.instance.Delay2Pool(GetType(), GameEffect, 5);
        //            Destroy(map[i, j].gameObject);
        //        }
        //        map[i, j] = null;

        //    }
        //}

    }


    //IEnumerator ClearReicol()
    //{
    //    // yield return null;

    //    for (int j = 3; j >=0; j--)
    //    {
    //        for (int i = 0; i < Originalmap02.GetLength(1); i++)
    //        {
    //            if (Originalmap02[i, j] != null)
    //            {
    //                Transform block = Originalmap02[i, j];
    //                Originalmap02[i, j] = null;
    //                GameObject GameEffect = PoolManager.instance.LoadObj(GetType(), transform);
    //                GameEffect.transform.position = block.position;
    //                PoolManager.instance.Delay2Pool(GetType(), GameEffect, 5);
    //                DestroyImmediate(block.gameObject);
                   
    //            }
    //            yield return new WaitForSeconds(0.03f);

    //        }
    //    }
       
        
    //}


    /// <summary>
    /// 生成形状
    /// </summary>
    public void SpawnShape()
    {
        if (gameState == GameState.Stop)
        {
            return;
        }
        RandomShape();
        for (int i = 0; i < 3; i++)
        {
            ShapePreview[i] = GameObject.Instantiate(Lib.instance.Shapes[ShapeIndex[i]]).transform;
            ShapePreview[i].SetParent(birthPviot[i]);
            ShapePreview[i].localPosition = Vector3.zero;
           // ShapePreview[i].localScale = Vector3.one * 0.4f;
            ShapePreview[i].DOScale(new Vector3(0, 0, 0), 0f);
        }
        for (int i = 0; i < 3; i++)
        {
           ShapePreview[i].DOScale(new Vector3(0.4f, 0.4f, 0), 0.5f).SetEase(Ease.OutBack).SetLoops(1, LoopType.Yoyo);
        }
    }

    public  int rand(int[] rate, int total)
    {
        int rand = UnityEngine.Random.Range(0, total);
        for (int i = rate.Length-1; i >=0 ; i--)
        {
            rand -= rate[i];
            if (rand <= 0)
            {
                return i;  
            }
        }
        return 0;
    }

    /// <summary>
    /// 随机形状
    /// </summary>
    public void RandomShape()
    {
        if (PlayerPrefs.GetInt("Score", 0) <= 300&& PlayerPrefs.GetInt("Score", 0)>=0)
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeIndex[i] = rand(rand01, 100);
               
            }
        }
        else if (PlayerPrefs.GetInt("Score", 0) <= 800 && PlayerPrefs.GetInt("Score", 0) > 300)
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeIndex[i] = rand(rand02, 100);
            }
        }
        else if (PlayerPrefs.GetInt("Score", 0) <= 1500 && PlayerPrefs.GetInt("Score", 0) > 800)
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeIndex[i] = rand(rand03, 100);
            }
        }
        else if ( PlayerPrefs.GetInt("Score", 0) > 1500)
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeIndex[i] = rand(rand04, 100);
            }
        }
    }
    /// <summary>
    /// 存储结果
    /// </summary>
    /// <param name="shape"></param>

    public void SaveShape(Transform shape)
    {
        
        for (int i = 0; i < shape.childCount; i++)
        {
            Vector3 childPos = shape.GetChild(i).position;
           // facade.SendNotification("MSG_DisShadow");
            //  Debug.Log("blockSpace=========" + blockSpace);
            //  Vector3 childPos = worldPos.TransformPoint(changePos);
            xIndex = (int)Math.Round((childPos.x + xPosx.x) / spacing);
            yIndex = (int)Math.Round((childPos.y + (-MPosy.y)) / spacing);
            //Debug.Log("qqqqq======" + ((childPos.x + MaxPosx.position.x) / blockSpace) + "===wwwwww========" + ((childPos.y + (-MinPosy.position.y)) / blockSpace));
            //Debug.Log("ddddx======" + childPos.x + "===fffffy========" + childPos.y);
            //求出在地图对应的格子
           // Debug.Log("xIndex=========" + xIndex + "====yIndex=========" + yIndex);
            if (xIndex >= 7) { xIndex = 7; }
            if (yIndex >= 7) { yIndex = 7; }
            if (xIndex <= 0) { xIndex = 0; }
            if (yIndex <= 0) { yIndex = 0; }
            map[xIndex, yIndex] = shape.GetChild(i);

            
        }

        CheckClear();

        //string msg = "";
        //for (int i = map.GetLength(1) - 1; i >= 0; i--)
        //{
        //    msg += "\n";
        //    for (int j = 0; j < map.GetLength(0); j++)
        //    {
        //        if (map[j, i] == null)
        //        {
        //            msg += "0,";
        //        }
        //        else
        //        { msg += "1,"; }
        //    }
        //}
        //Debug.Log(msg);
    }

    /// <summary>
    /// 方块消除检测
    /// </summary>
    public void CheckClear()
    {
        List<int> rowIndex = new List<int>();
        List<int> colIndex = new List<int>();
        //消除行
        for (int i = 0; i < map.GetLength(1); i++)
        {
            bool isClearRow = true;
            for (int j = 0; j < map.GetLength(0); j++)
            {
                if (map[j, i] == null)
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
        //消除列
        for (int i = 0; i < map.GetLength(0); i++)
        {
            bool isClearCol = true;
            for (int j = 0; j < map.GetLength(1); j++)
            {

                if (map[i, j] == null)
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

        AssignMap02();
        if (rowIndex.Count >= 1 && colIndex.Count >= 1)
        {

            ClearShapeRow(rowIndex);
            ClearShapeCol(colIndex);
            CountScore(rowIndex.Count+colIndex.Count);
            PlayEliminateMusic(rowIndex.Count + colIndex.Count);
            DoUIGame.instance.ShowNum(rowIndex.Count + colIndex.Count);
        }
        else if (colIndex.Count >= 1)
        {
           
            ClearShapeCol(colIndex);
            CountScore(colIndex.Count);
            PlayEliminateMusic( colIndex.Count);
            DoUIGame.instance.ShowNum(colIndex.Count);
        }
        else if (rowIndex.Count >= 1)
        {
            
            ClearShapeRow(rowIndex);
            CountScore(rowIndex.Count);
            PlayEliminateMusic(rowIndex.Count);
            DoUIGame.instance.ShowNum(rowIndex.Count);
        }
    }

    /// <summary>
    /// 计算分数
    /// </summary>
    public void CountScore(int num)
    {
        if (num==1) { Score= Score +20; }
        else if (num == 2) { Score = Score + 50; }
        else if (num == 3) { Score = Score + 100; }
        else if (num == 4) { Score = Score + 200; }
        else if (num == 5) { Score = Score + 300; }
        else if (num == 6) { Score = Score + 500; }
        PlayerPrefs.SetInt("Score", Score);
        txtScore.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }

    public void PlayEliminateMusic(int num)
    {
        if (num == 1) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[0]); }
        else if (num == 2) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[1]); }
        else if (num == 3) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[2]); }
        else if (num == 4) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[3]); }
        else if (num == 5) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[4]); }
        else if (num == 6) { AudioManager.Instance.PlaySFX(Lib.instance.Eliminate_sound[4]); }
    }
    /// <summary>
    /// 计算最高分
    /// </summary>
    /// <param name="Prebest"></param>
    /// <param name="Score"></param>
    public void BestScore(int Prebest,int Score)
    {
        if (Prebest >= Score)
        {
            txtBest.text = Prebest.ToString();
        }
        else
        {
            txtBest.text = Score.ToString();
            bestScore = Score;
            PlayerPrefs.SetInt("bestScore", Score);
        }
    }

    GType GetType()
    {
        GType type = GType.BLUE;
        switch (ColorIndex)
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
    /// <summary>
    /// 消除方块
    /// </summary>
    void ClearShapeRow(List<int> rcIndex)
    {
        for (int i = 0; i < rcIndex.Count; i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                if (map[j, rcIndex[i]] != null)
                {
                    map[j, rcIndex[i]] = null;
                }

            }
        }
        for (int i = 0; i < rcIndex.Count; i++)
        {
            StartCoroutine(ClearShapeRow(rcIndex[i]));
        }
        
    }

    IEnumerator ClearShapeRow(int i)
    {
        for (int j = 0; j < Originalmap02.GetLength(0); j++)
        {
            if (Originalmap02[j, i] != null)
            {

                Transform block = Originalmap02[j, i];
                Originalmap02[j, i] = null;
                GameObject GameEffect = PoolManager.instance.LoadObj(GetType(), transform);
                GameEffect.transform.position = block.position;
                PoolManager.instance.Delay2Pool(GetType(), GameEffect, 5);
                DestroyImmediate(block.gameObject);
          
            }
            yield return new WaitForSeconds(0.05f);
        }
        facade.SendNotification("MSG_ShapeGray");
    }

    void ClearShapeCol(List<int> rcIndex)
    {
        for (int i = 0; i < rcIndex.Count; i++)
        {
            for (int j = map.GetLength(0) - 1; j >= 0; j--)
            {
                if (map[rcIndex[i], j] != null)
                {
                    map[rcIndex[i], j] = null;
                }
            }
        }
        for (int i = 0; i < rcIndex.Count; i++)
        {
            StartCoroutine(ClearShapecol(rcIndex[i]));
        }
        

    }

    IEnumerator ClearShapecol(int i)
    {
        yield return null;
        
        for (int j = 0; j < Originalmap02.GetLength(0); j++)
        {
            if (Originalmap02[i, j] != null)
            {
                Transform block = Originalmap02[i, j];
                Originalmap02[i, j] = null;
                GameObject GameEffect = PoolManager.instance.LoadObj(GetType(), transform);
                GameEffect.transform.position = block.position;
                PoolManager.instance.Delay2Pool(GetType(), GameEffect, 5);
                DestroyImmediate(block.gameObject);
 
            }
            yield return new WaitForSeconds(0.05f);
        }
        facade.SendNotification("MSG_ShapeGray");
    }


    /// <summary>
    /// 检测是否超出边界
    /// </summary>
    public bool CheckPos(Transform shape)
    {
        for (int i = 0; i < shape.childCount; i++)
        {

            Vector3 childPos = shape.GetChild(i).position;

            if ((childPos.x > MinPosx.position.x && childPos.x < MaxPosx.position.x)
                 && (childPos.y > MinPosy.position.y && childPos.y < MaxPosy.position.y))
            {
                return true;
            }


        }

        return false;
    }
    /// <summary>
    /// 将map复制到Originalmap
    /// </summary>
    public void AssignMap()
    {
        for (int i = 0; i < map.GetLength(1); i++)
        {
           
            for (int j = 0; j < map.GetLength(0); j++)
            {
                Originalmap[i, j] = map[i, j];
            }

        }
    }

    /// <summary>
    /// 将map复制到Originalmap02
    /// </summary>
    public void AssignMap02()
    {
        for (int i = 0; i < map.GetLength(1); i++)
        {

            for (int j = 0; j < map.GetLength(0); j++)
            {
                Originalmap02[i, j] = map[i, j];
            }

        }
    }

    void OnBtnSettingClick()
    {
        UIManager.Instance.OpenUI(EUITYPE.UISetting);
        PlayClickAudio();
    }
    
    /// <summary>
    /// 五星好评
    /// </summary>
    public void StarGreat()
    {
        if (PlayerPrefs.GetInt("play_times", 0) == 1 || PlayerPrefs.GetInt("play_times", 0) == 2 ||
            PlayerPrefs.GetInt("play_times", 0) == 4 || PlayerPrefs.GetInt("play_times", 0) == 5 ||
            PlayerPrefs.GetInt("play_times", 0) == 8 || PlayerPrefs.GetInt("play_times", 0) == 9 ||
            PlayerPrefs.GetInt("play_times", 0) == 10 || PlayerPrefs.GetInt("play_times", 0) == 15)
        {
            SDKManager.Instance.FiveStars();
        }
    }

    public void NewBirth01()
    {
        ShapePreview[1] = GameObject.Instantiate(Lib.instance.Shapes[15]).transform;
        ShapePreview[1].SetParent(birthPviot[1]);
        ShapePreview[1].localPosition = Vector3.zero;
        ShapePreview[1].localScale = Vector3.one * 0.4f;
    }
    public void NewBirth02()
    {
        ShapePreview[1] = GameObject.Instantiate(Lib.instance.Shapes[14]).transform;
        ShapePreview[1].SetParent(birthPviot[1]);
        ShapePreview[1].localPosition = Vector3.zero;
        ShapePreview[1].localScale = Vector3.one * 0.4f;
    }
    public void NewBirth03()
    {
        ShapePreview[1] = GameObject.Instantiate(Lib.instance.Shapes[13]).transform;
        ShapePreview[1].SetParent(birthPviot[1]);
        ShapePreview[1].localPosition = Vector3.zero;
        ShapePreview[1].localScale = Vector3.one * 0.4f;
    }




    void ShowAd()
    {

#if UNITY_EDITOR
        //StartGame();
#else
        if (KomalUtil.Instance.IsNetworkReachability()&&Client.Instance.HaveAd)
        { 
            SDKManager.Instance.ShowInterstitial((result) =>
            {
                if (result == InterstitialResult.DISMISS)
                {
                   
                     Time.timeScale = 1;
                    AudioManager.Instance.PauseAllListener(false);
                   
                }
                else if (result == InterstitialResult.DISPLAY)
                {
                     Time.timeScale = 0;
                    AudioManager.Instance.PauseAllListener(true);
                }

            });
        }
        else { StartGame(); };
#endif
    }
}





enum GameState
{
    Start,
    Stop
}

