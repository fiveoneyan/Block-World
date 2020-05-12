using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoBoomAndHand : MonoBehaviour
{
    public Image boom;
    public Image hand;
    // Start is called before the first frame update
    void Start()
    {
        Sequence seqBoom = DOTween.Sequence();
        seqBoom.Append(boom.transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f));
        seqBoom.Append(boom.transform.DOScale(new Vector3(1f, 1f, 0), 0.5f));
        seqBoom.SetLoops(-1);

        Sequence seqHand = DOTween.Sequence();
        seqHand.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, -50, 0), 0.4f));
        seqHand.Append(hand.DOFade(1, 0f));
        seqHand.Append(hand.transform.DOBlendableLocalMoveBy(new Vector3(0, 0, 0), 0.2f));
        seqHand.Append(hand.DOFade(0, 0.3f));
       
        seqHand.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
