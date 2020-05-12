using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal;
using komal.sdk;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
 {
    public Transform bg;
    public Button btnClose;
    public Button btnSound;
    public Button btnVibration;
    public Button btnRestart;
    public Button btnSupport;
    public Image panel;
    public Image[] imgSound;
    public Image[] imgVibration;

    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        bg.DOBlendableLocalMoveBy(new Vector3(0, 1000, 0), 0f);
        bg.DOBlendableLocalMoveBy(new Vector3(0, -1000, 0), 0.7f).SetEase(Ease.OutBack).SetLoops(1, LoopType.Yoyo);
        btnClose.onClick.AddListener(OnBtnCloseClick);
        btnSound.onClick.AddListener(OnBtnSoundClick);
        btnVibration.onClick.AddListener(OnBtnVibrationClick);
        btnRestart.onClick.AddListener(OnBtnRestartClick);
        btnSupport.onClick.AddListener(OnBtnSupportClick);
        if (AudioManager.Instance.GetBGMVolume() <= 0)
        {
            imgSound[1].gameObject.SetActive(true);
            imgSound[0].gameObject.SetActive(false);
            //btnSound.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            imgSound[1].gameObject.SetActive(false);
            imgSound[0].gameObject.SetActive(true);
            //btnSound.GetComponent<Image>().color = Color.white;
        }

        if (PlayerPrefs.GetInt("Shake", 1) == 1)
        {
            imgVibration[1].gameObject.SetActive(false);
            imgVibration[0].gameObject.SetActive(true);
            //btnVibration.GetComponent<Image>().color = Color.white;
        }
        else
        {
            imgVibration[1].gameObject.SetActive(true);
            imgVibration[0].gameObject.SetActive(false);
            //btnVibration.GetComponent<Image>().color = Color.gray; 
        }
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
    }
    #endregion

    void Init(params object[] param)
    {
      
    }
    void OnBtnCloseClick()
    {
        PlayClickAudio();
        panel.DOFade(0,1f);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, -80, 0), 0.1f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo));
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, 2000, 0), 0.7f))
        .OnComplete(() =>
        {
            UIManager.Instance.CloseUI(this);
        });
        

    }
    void OnBtnSoundClick()
    {
       
        PlayClickAudio();
        if (AudioManager.Instance.GetBGMVolume() > 0)
        {
            //imgSound[1].gameObject.SetActive(true);
            //imgSound[0].gameObject.SetActive(false);
          //  btnSound.GetComponent<Image>().color = Color.gray;
            AudioManager.Instance.SetBGMVolume(0);
        }
        else
        {
            //imgSound[1].gameObject.SetActive(false);
            //imgSound[0].gameObject.SetActive(true);
           // btnSound.GetComponent<Image>().color = Color.white;
            AudioManager.Instance.SetBGMVolume(1);
        }
        if (AudioManager.Instance.GetSFXVolume() > 0)
        {
            imgSound[1].gameObject.SetActive(true);
            imgSound[0].gameObject.SetActive(false);
            //  btnSound.GetComponent<Image>().color = Color.gray;
            AudioManager.Instance.SetSFXVolume(0);
        }
        else
        {
            imgSound[1].gameObject.SetActive(false);
            imgSound[0].gameObject.SetActive(true);
          //  btnSound.GetComponent<Image>().color = Color.white;
            AudioManager.Instance.SetSFXVolume(1);
        }
    }
    void OnBtnVibrationClick()
    {
        PlayClickAudio();
        if (PlayerPrefs.GetInt("Shake", 1) >0)
        {
            imgVibration[1].gameObject.SetActive(true);
            imgVibration[0].gameObject.SetActive(false);
           // btnVibration.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetInt("Shake", 0);
        }
        else
        {
            imgVibration[1].gameObject.SetActive(false);
            imgVibration[0].gameObject.SetActive(true);
           // btnVibration.GetComponent<Image>().color = Color.white;
            PlayerPrefs.SetInt("Shake", 1);
        }
    }
    void OnBtnRestartClick()
    {
        PlayClickAudio();
        ShowAd();
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, -80, 0), 0.1f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo));
        seqAfter.Append(bg.DOBlendableLocalMoveBy(new Vector3(0, 2000, 0), 0.7f))
        .OnComplete(() =>
        {
            UIManager.Instance.CloseUI(this);
            DoUIGame.instance.StarCortoon();
            facade.SendNotification("MSG_GameOver");
            UIGame.instance.ReStartClear();
            facade.SendNotification("MSG_GameStart");
        });
        
       
       
    }
    void OnBtnSupportClick()
    {
        PlayClickAudio();
        //btnSound.GetComponent<Image>().color = Color.gray;
        Application.OpenURL(Config.ID.GetValue("AppUrl"));
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
        else {  };
#endif
    }
}
