using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGround : MonoBehaviour
{
    public Transform[,] Groundmap = new Transform[8, 8];
    public Transform[] Rowmap01;
    public Transform[] Rowmap02;
    public Transform[] Rowmap03;
    public Transform[] Rowmap04;
    public Transform[] Rowmap05;
    public Transform[] Rowmap06;
    public Transform[] Rowmap07;
    public Transform[] Rowmap08;
    public static SaveGround instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        for (int i = 0; i < Groundmap.GetLength(1); i++)
        {
            Groundmap[i, 0] = Rowmap01[i];
            Groundmap[i, 1] = Rowmap02[i];
            Groundmap[i, 2] = Rowmap03[i];
            Groundmap[i, 3] = Rowmap04[i];
            Groundmap[i, 4] = Rowmap05[i];
            Groundmap[i, 5] = Rowmap06[i];
            Groundmap[i, 6] = Rowmap07[i];
            Groundmap[i, 7] = Rowmap08[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
