using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoHand : MonoBehaviour
{
    public Image hand;
    // Start is called before the first frame update
    void Start()
    {

        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, 550, 0), 1f));
        seqAfter.Append(hand.DOFade(0, 0.5f));
        seqAfter.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, -550, 0), 1f));
        seqAfter.Append(hand.DOFade(1, 0.2f));
        seqAfter.SetLoops(-1);
        //StartCoroutine(WaitAmoment());

    }

    IEnumerator WaitAmoment()
    {
        yield return new WaitForSeconds(0.0001f);
        Sequence seqAfter = DOTween.Sequence();
        seqAfter.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, 550, 0), 0.8f));
        seqAfter.Append(hand.DOFade(0, 0.5f));
        seqAfter.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, -550, 0), 0.5f));
        seqAfter.Append(hand.DOFade(1, 0.2f));
        seqAfter.SetLoops(-1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
     
}
