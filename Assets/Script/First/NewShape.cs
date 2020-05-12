using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewShape : MonoBehaviour
{
    public Transform ShapePreview;
    public Transform birthPviot;
    public Transform NewVersion;
    public static NewShape instance;
    // Start is called before the first frame update
    void Start()
    {
        instance=this ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewManLead()
    {
        
            GameObject NewShape03 = PoolManager.instance.LoadObj(GType.NewShape03, NewVersion);
            NewShape03.transform.position = new Vector3(0, 145, 0);
            NewBirth03();
            UIGame.instance.isFirstTimes = false;
    }

    public void NewBirth01()
    {
        ShapePreview = GameObject.Instantiate(Lib.instance.Shapes[9]).transform;
        ShapePreview.SetParent(birthPviot);
        ShapePreview.localPosition = Vector3.zero;
        ShapePreview.localScale = Vector3.one * 0.4f;
    }
    public void NewBirth02()
    {
        ShapePreview = GameObject.Instantiate(Lib.instance.Shapes[8]).transform;
        ShapePreview.SetParent(birthPviot);
        ShapePreview.localPosition = Vector3.zero;
        ShapePreview.localScale = Vector3.one * 0.4f;
    }
    public void NewBirth03()
    {
        ShapePreview = GameObject.Instantiate(Lib.instance.Shapes[17]).transform;
        ShapePreview.SetParent(birthPviot);
        ShapePreview.localPosition = Vector3.zero;
        ShapePreview.localScale = Vector3.one * 0.4f;
    }
}
