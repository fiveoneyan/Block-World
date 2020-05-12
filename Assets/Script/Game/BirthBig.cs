using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BirthBig : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(0,0);
        transform.DOScale(1, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
