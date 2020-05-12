using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoUIGame : MonoBehaviour
{
    public Transform up;
    public Transform Map;
    public Transform[] ImgdownPi;
    public Transform[] ShapePi=new Transform[3];
    public Image[] num;
    public Image[] imgGood;
    public static DoUIGame instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StarCortoon();
        for (int i = 0; i < imgGood.Length; i++)
        {
            imgGood[i].transform.DOScale(0, 0);
        }
        for (int i = 0; i < num.Length; i++)
        {
            num[i].transform.DOScale(0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNum(int i)
    {
       // CameraShakeEffect.instance.Shake();
        StartCoroutine(ShowGood(i));
        if (i == 1)
        {
            StartCoroutine(ShowNumber(0, 2, 0));
            
        }
        else if (i == 2)
        {
            StartCoroutine(ShowNumber(0, 5, 0));
            
        }
        else if (i == 3)
        {
            StartCoroutine(ShowNumber(1, 0, 0));
            
        }
        else if (i == 4)
        {
            StartCoroutine(ShowNumber(2, 0, 0));
            
        }
        else if (i == 5)
        {
            StartCoroutine(ShowNumber(3, 0, 0));
            
        }
        else if (i == 6)
        {
            StartCoroutine(ShowNumber(5, 0, 0));
            
        }
        if (i == 3)
        {
            CameraShakeEffect.instance.Shake();
        }
        
    }

    IEnumerator ShowNumber(int a ,int b,int c)
    {
        
        if (a > 0)
        { num[0].transform.DOScale(1, 0.2f);
            num[0].DOFade(1, 0);
        }
        num[1].transform.DOScale(1, 0.2f);
        num[2].transform.DOScale(1, 0.2f);
        num[1].DOFade(1, 0);
        num[2].DOFade(1, 0);
        num[0].sprite = Lib.instance.imgNumber[a];
        num[1].sprite = Lib.instance.imgNumber[b];
        num[2].sprite = Lib.instance.imgNumber[c];
        yield return new WaitForSeconds(0.7f);
        num[0].DOFade(0, 0.5f);
        num[1].DOFade(0, 0.5f);
        num[2].DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        num[0].transform.DOScale(0, 0);
        num[1].transform.DOScale(0, 0);
        num[2].transform.DOScale(0, 0);
    }
    IEnumerator ShowGood(int i)
    {
        if (i >= 2)
        {
            imgGood[i - 2].transform.DOScale(1, 0.3f);
            imgGood[i - 2].DOFade(1, 0);
            yield return new WaitForSeconds(0.7f);
            imgGood[i - 2].DOFade(0, 0.7f);
            yield return new WaitForSeconds(0.7f);
            imgGood[i - 2].transform.DOScale(0, 0);
        }
    }
    public void GameCartoon()
    {
        up.DOBlendableLocalMoveBy(new Vector3(0, 200, 0), 0f);
        for (int i = 0; i < ImgdownPi.Length; i++)
        {
            ImgdownPi[i].DOScale(new Vector3(0, 0, 0), 0f);
            ShapePi[i].DOScale(new Vector3(0, 0, 0), 0f);
        }

        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(up.DOBlendableLocalMoveBy(new Vector3(0, -200, 0), 1f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo));

        DownShow(ImgdownPi, ShapePi);

    }


void DownShow(Transform[] ImgdownPi, Transform[] ShapePi)
{

        for (int i = 0; i < ImgdownPi.Length; i++)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(ImgdownPi[i].DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo));
            mySequence.Append(ShapePi[i].DOScale(new Vector3(1, 1, 1), 0.5f));

        }

}
    public void PuGround()
    {
        float a = 0.01f;
        float d = 0.01f;
        float q = 0.01f;
        for (int i = 0; i < SaveGround.instance.Groundmap.GetLength(1); i++)
        {
            for (int j = 0; j < SaveGround.instance.Groundmap.GetLength(0); j++)
            {
                SaveGround.instance.Groundmap[i,j].DOBlendableLocalMoveBy(new Vector3(-800, 0, 0), 0);
            }
        }
        int c=0, b=7; 
        while (c < 8 && b>=0)
        {
            d = a;
            q = a;
            SaveGround.instance.Groundmap[c , b ].DOBlendableLocalMoveBy(new Vector3(800, 0, 0), a);
            for (int j = b-1; j >= 0; j--)
            {
                
                SaveGround.instance.Groundmap[c, j].DOBlendableLocalMoveBy(new Vector3(800, 0, 0), d);
                d = 0.05f + d;
            }
            for (int j = c+1; j < 8; j++)
            {
                SaveGround.instance.Groundmap[j, b].DOBlendableLocalMoveBy(new Vector3(800, 0, 0), q);
                q = 0.05f + q;
            }
            a = 0.1f + a;
            c++;
            b--;
        }
    }
    IEnumerator waitAmoment()
    {
       
        yield return new WaitForSeconds(3);
       
        StopCoroutine(waitAmoment());
    }
    public void StarCortoon()
    {
        PuGround();
        StartCoroutine(waitAmoment());
        GameCartoon();
        
    }

}
