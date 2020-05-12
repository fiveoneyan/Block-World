using System.Collections;
using System.Collections.Generic;
using komal.sdk;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            SDKManager.Instance.ShowRecommend((result) =>
            {

            });
        }
    }
}
