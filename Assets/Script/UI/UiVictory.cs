using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiVictory : UIBase
 {

    public Transform jiangbei;
    public Image New;
    public Image Record;
    public Image Guanquan;
    public AudioClip New_sound;
    public ParticleSystem Lihua;
   // public ParticleSystem zhongLihua;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        ShowCartoon();
        StartCoroutine(CloseUIVictory());
        AudioManager.Instance.PlaySFX(New_sound);
        //  zhongLihua.Play();
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
    public void ShowCartoon()
    {
        New.DOFade(0, 0);
        Record.DOFade(0, 0);
        Guanquan.DOFade(0, 0);
        jiangbei.DOBlendableLocalMoveBy(new Vector3(0, 1000, 0), 0);
        New.DOFade(1, 2f);
        Record.DOFade(1, 2f);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(jiangbei.DOBlendableLocalMoveBy(new Vector3(0, -1000, 0), 0.5f).SetEase(Ease.OutBack).SetLoops(1, LoopType.Yoyo))
       .OnComplete(() =>
       {
           Lihua.Play();
       });
        Guanquan.transform.DOBlendableRotateBy(new Vector3(30, 30, 30), 5).SetLoops(1, LoopType.Yoyo);
        Guanquan.DOFade(1, 2f);
    }
    IEnumerator CloseUIVictory()
    {
        yield return new WaitForSeconds(3.5f);
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI(EUITYPE.UIDefeat);
        StopCoroutine(CloseUIVictory());
    }
}
