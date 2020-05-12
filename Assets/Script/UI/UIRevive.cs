using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal;
using komal.sdk;
using UnityEngine;
using UnityEngine.UI;

public class UIRevive : UIBase
 {
    public Button btnRevive;
    public Button btnNo;
    public Transform NoThanks;
    public Transform bg;
    public int Second;
    public Text txtTime;
    public Image panel;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        UIGame.instance.Revivetimes++;
        ReviveTimes();
        Second = 5;
        ReviveCartoon();
        btnRevive.onClick.AddListener(OnBtnReviveClick);
        btnNo.onClick.AddListener(OnBtnNoClick);
        
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
        btnRevive.onClick.RemoveListener(OnBtnReviveClick);
        btnNo.onClick.RemoveListener(OnBtnNoClick);
    }
    #endregion

    void Init(params object[] param)
    {
      
    }

    public void ReviveTimes()
    {
        if (UIGame.instance.Revivetimes % 2 == 0)    
        {
            
            UIManager.Instance.CloseUI(this);
            facade.SendNotification("MSG_GameOver");
            facade.SendNotification("MSG_GameBeGray");
           
        }
        
    }

    void OnBtnReviveClick()
    {
        StopCoroutine("Timer");
        PlayClickAudio();
        Lib.instance.ShowRewardedVideo((boo)=>
        {
            if (boo==true)
            {
                panel.DOFade(0, 1f);
                NoThanks.DOBlendableLocalMoveBy(new Vector3(0, -500, 0), 0.5f);
                Sequence seqAfter = DOTween.Sequence();
                seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 0.1f));
                seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, 1500, 0), 0.5f))
                .OnComplete(() =>
                {
                    UIManager.Instance.CloseUI(this);
                    facade.SendNotification("MSG_GameRevive");
                });
            }
            else {
                StartCoroutine("Timer");
            }
        });     
    }
    void OnBtnNoClick()
    {
        PlayClickAudio();
        ShowAd();
        panel.DOFade(0, 1f);
        NoThanks.DOBlendableLocalMoveBy(new Vector3(0, -500, 0), 0.5f);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 0.1f));
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, 1500, 0), 0.5f))
        .OnComplete(() =>
        {
            if (PlayerPrefs.GetInt("bestScore", 0) < PlayerPrefs.GetInt("Score", 0))
            {
                facade.SendNotification("MSG_GameOver");
              //  facade.SendNotification("MSG_GameStart");
              //  UIManager.Instance.OpenUI(EUITYPE.UIVictory);
                UIManager.Instance.CloseUI(this);
                // UIManager.Instance.CloseUI(EUITYPE.UIGame);
                facade.SendNotification("MSG_GameBeGray");
            }
            else
            {
                facade.SendNotification("MSG_GameOver");
              //  facade.SendNotification("MSG_GameStart");
              //  UIManager.Instance.OpenUI(EUITYPE.UIDefeat);
                UIManager.Instance.CloseUI(this);
                //UIManager.Instance.CloseUI(EUITYPE.UIGame);
                facade.SendNotification("MSG_GameBeGray");
            }
        });
        

    }


    public void ReviveCartoon()
    {
        
        bg.DOBlendableLocalMoveBy(new Vector3(0, 500, 0), 0f);
        btnNo.gameObject.SetActive(false);
        bg.DOBlendableLocalMoveBy(new Vector3(0, -500, 0), 1f).SetEase(Ease.OutBack).SetLoops(1, LoopType.Yoyo);
        StartCoroutine("ShawNothanks");
        StopCoroutine("Timer");
        StartCoroutine("Timer");
    }

    IEnumerator ShawNothanks()
    {
        yield return new WaitForSeconds(3);
        btnNo.gameObject.SetActive(true);
        StopCoroutine("ShawNothanks");
    }
    IEnumerator Timer()
    {
        txtTime.text = Second.ToString();
        Second--;
        AudioManager.Instance.PlaySFX(Lib.instance.Countdown_sound);
        yield return new WaitForSeconds(1);

        if (Second < 0)
        {
            panel.DOFade(0, 1f);
            NoThanks.DOBlendableLocalMoveBy(new Vector3(0, -500, 0), 0.5f);
            Sequence seqAfter = DOTween.Sequence();
            seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 0.1f));
            seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, 1500, 0), 0.5f))
            .OnComplete(() =>
            {
                if (PlayerPrefs.GetInt("bestScore", 0) < PlayerPrefs.GetInt("Score", 0))
                {
                    facade.SendNotification("MSG_GameOver");
                  //  facade.SendNotification("MSG_GameStart");
                 //   UIManager.Instance.OpenUI(EUITYPE.UIVictory);
                    UIManager.Instance.CloseUI(this);
                    //UIManager.Instance.CloseUI(EUITYPE.UIGame);
                    facade.SendNotification("MSG_GameBeGray");
                }
                else
                {
                    facade.SendNotification("MSG_GameOver");
                  //  facade.SendNotification("MSG_GameStart");
                  //  UIManager.Instance.OpenUI(EUITYPE.UIDefeat);
                    UIManager.Instance.CloseUI(this);
                    // UIManager.Instance.CloseUI(EUITYPE.UIGame);
                    facade.SendNotification("MSG_GameBeGray");
                }
            });

        }
        else { StartCoroutine("Timer"); }
    }

    void ShowAd()
    {

#if UNITY_EDITOR

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
        else {  };
#endif
    }
}
