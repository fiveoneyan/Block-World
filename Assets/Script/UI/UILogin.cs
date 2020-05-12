using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : UIBase
{
    public bool isbb; 
    public Image progressBar;
    private int curProgressValue = 0;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        bb();
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

    // Update is called once per frame
    void Update()
    {
        int progressValue = 100;

        if (curProgressValue < progressValue)
        {
            curProgressValue++;
        }
        progressBar.fillAmount = curProgressValue / 100f;//实时更新滑动进度图片的fillAmount值  
    }

    IEnumerator aa()
    {
        yield return new WaitForSeconds(2);
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI(EUITYPE.UIGame);
        StopCoroutine(aa());

    }



    public void bb()
    {
        StartCoroutine(aa());

    }
}
