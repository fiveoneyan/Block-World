using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShakeEffect : MonoBehaviour
{

    public static CameraShakeEffect instance;
    void Start()
    {
        instance = this;
    }
    public void Shake()
    {
        
        transform.DOShakePosition(0.5f, new Vector3(30, 30, 0)); // 平面进行一个震动 只改变x,y没有改变z (值是一个震动的强度)
    }

   
    //private Camera mainCamera;
    ////震动标志位
    //public bool isShakeCamera = false;
    ////震动幅度
    //public float shakeLevel = 3f;
    ////震动时间
    //public float setShakeTime = 0.2f;
    ////震动的FPS
    //public float shakeFps = 45f;

    //private float fps;
    //private float shakeTime = 0.0f;
    //private float frameTime = 0.0f;
    //private float shakeDelta = 0.005f;

    //void Awake()
    //{
    //    //获取Camera组件
    //    mainCamera = GetComponent<Camera>();
    //}

    //void Start()
    //{
    //    shakeTime = setShakeTime;
    //    fps = shakeFps;
    //    frameTime = 0.03f;
    //    shakeDelta = 0.005f;
    //}

    //void Update()
    //{
    //    if (isShakeCamera)
    //    {
    //        if (shakeTime > 0)
    //        {
    //            shakeTime -= Time.deltaTime;
    //            if (shakeTime <= 0)
    //            {
    //                mainCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
    //                isShakeCamera = false;
    //                shakeTime = setShakeTime;
    //                fps = shakeFps;
    //                frameTime = 0.03f;
    //                shakeDelta = 0.005f;
    //            }
    //            else
    //            {
    //                frameTime += Time.deltaTime;
    //                if (frameTime > 1.0 / fps)
    //                {
    //                    frameTime = 0;
    //                    mainCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value),
    //                        shakeDelta * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
    //                }
    //            }
    //        }
    //    }
    //}

    //public void Shake()
    //{
    //    isShakeCamera = true;
    //}
}
