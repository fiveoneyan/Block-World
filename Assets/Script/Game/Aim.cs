using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform MaxPosx;
    public Transform MinPosx;
    public Transform MaxPosy;
    public Transform MinPosy;
    public static Aim instance;
    void Awake()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
