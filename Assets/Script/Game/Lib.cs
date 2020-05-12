using System;
using System.Collections;
using System.Collections.Generic;
using komal;
using komal.sdk;
using UnityEngine;

public  class Lib :MonoBehaviour
{
    public static Lib instance;
    public Dictionary<int, GameObject> Shapes = new Dictionary<int, GameObject>();//block形状
    public  Dictionary<int, Sprite> dicColor = new Dictionary<int, Sprite>();//颜色
    public Dictionary<int, GameObject> Effect = new Dictionary<int, GameObject>();//block形状
    public Sprite[] imgColor;
    //public Sprite[] FlagB;
    public GameObject[] Shape;
    public GameObject[] effect;
    public Sprite Ground;
   // public AudioClip Eliminate_sound;
    public AudioClip UP_sound;
    public AudioClip Down_sound;
    public AudioClip Error_sound;
    public AudioClip Score_sound;
    public AudioClip Fail_sound;
    public AudioClip Countdown_sound;
    public AudioClip New_sound;
    public AudioClip Btn_sound;
    public AudioClip Gray_sound;
    public AudioClip Bg_sound;
    public Move move;
    
    public AudioClip[] Eliminate_sound;
    public Sprite[] imgNumber;
   
    void Awake()
    {
        instance = this;
        for (int i = 0; i < imgColor.Length; i++)
        {
            dicColor.Add(i,imgColor[i]);
        }
        for (int i = 0; i < Shape.Length; i++)
        {
            Shapes.Add(i, Shape[i]);
        }
        for (int i = 0; i < effect.Length; i++)
        {
            Effect.Add(i, effect[i]);
        }


    }

    public void ShowRewardedVideo(Action<bool> callback)
    {
        if (!KomalUtil.Instance.IsNetworkReachability())
            return;

#if UNITY_EDITOR
        callback(true);
#else
        bool isRewarded = false;
        SDKManager.Instance.ShowRewardedVideo((result) =>
        {
            if (result == RewardedVideoResult.REWARDED)
            {
                isRewarded = true;
            }
            else if (result == RewardedVideoResult.DISMISS)
            {
                Time.timeScale = 1;
                AudioManager.Instance.PauseAllListener(false);
                callback?.Invoke(isRewarded);
            }
            else if (result == RewardedVideoResult.DISPLAY)
            {
                Time.timeScale = 0;
                AudioManager.Instance.PauseAllListener(true);
            }
        });
#endif
    }

}
