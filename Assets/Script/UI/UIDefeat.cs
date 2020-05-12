using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal;
using komal.sdk;
using UnityEngine;
using UnityEngine.UI;

public class UIDefeat : UIBase
 {
    public Text txtScore;
    public Text txtBest;
    public Button btnStart;
    public Button btnRank;
    public Button btnDianzan;
    int result = 0;
    private Sequence mScoreSequence;
    private int mOldScore = 0;
    private int newScore = 0;
    private int mOldBest = 0;
    private int newBest = 0;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        UIGame.instance.Revivetimes = 0;
        btnStart.onClick.AddListener(OnBtnStartClick);
        btnRank.onClick.AddListener(OnBtnRankClick);
        btnDianzan.onClick.AddListener(OnBtnDianzanClick);
        mScoreSequence = DOTween.Sequence();
        mScoreSequence.SetAutoKill(false);
        UIGame.instance.StarGreat();
        ShawCartoon();
        SDKManager.Instance.RecordRank(Config.ID.GetValue("HighScore"), PlayerPrefs.GetInt("bestScore", 0));
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnPause()
    {
       base.OnPause();
    }

    public override void OnExit()
    {
       base.OnExit();
        btnStart.onClick.RemoveListener(OnBtnStartClick);
        btnRank.onClick.RemoveListener(OnBtnRankClick);
        btnDianzan.onClick.RemoveListener(OnBtnDianzanClick);
    }
    #endregion

    void Init(params object[] param)
    {
      
    }


    public void ShowNumber()
    {
       
    }
    void OnBtnStartClick()
    {
        PlayClickAudio();
        if (PlayerPrefs.GetInt("play_times", 0) % 3==0)
        {
            ShowAd();
        }
        UIManager.Instance.OpenUI(EUITYPE.UIGame);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(transform.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 0.1f))
                .Append(transform.DOBlendableLocalMoveBy(new Vector3(0, 1500, 0), 0.5f)).OnComplete(() =>
        {
            facade.SendNotification("MSG_GameStart");
            UIManager.Instance.CloseUI(this);
            DoUIGame.instance.StarCortoon();
        });
        
    }
    void OnBtnRankClick()
    {
        PlayClickAudio();
        SDKManager.Instance.OpenGameCenter();
    }
    void OnBtnDianzanClick()
    {
        PlayClickAudio();
        Application.OpenURL(Config.ID.GetValue("AppUrl"));
    }

    public void ShawCartoon()
    {
        AudioManager.Instance.PlaySFX(Lib.instance.Score_sound);
        transform.DOBlendableLocalMoveBy(new Vector3(0, 1200, 0), 0f);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(transform.DOBlendableLocalMoveBy(new Vector3(0, -1200, 0), 0.5f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo)).OnComplete(()=>
        {
            newScore += PlayerPrefs.GetInt("Score", 0);
            newBest += PlayerPrefs.GetInt("bestScore", 0);
            DigitalAnimation();
        });
    }

    void DigitalAnimation()
    {
        mScoreSequence.Append(DOTween.To(delegate (float value) {
            var temp = Math.Floor(value);
            txtScore.text = temp + "";
        }, mOldScore, newScore, 0.4f));
        mOldScore = newScore;
        mScoreSequence.Append(DOTween.To(delegate (float value) {
            var temp = Math.Floor(value);
            txtBest.text = temp + "";
        }, mOldBest, newBest, 0.4f));
        mOldBest = newBest;
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
                   
                   
                    AudioManager.Instance.PauseAllListener(false);
                   
                }
                else if (result == InterstitialResult.DISPLAY)
                {

                    AudioManager.Instance.PauseAllListener(true);
                }

            });
        }
        else { };
#endif
    }
}
