using System;
using Recommend;

namespace komal.sdk
{
    public class AppRecommendSDK : SDKBase, IAR
    {
        public void ShowRecommend(Action<RecommendResult> callback)
        {
            AppRecommend.Instance.ShowRecommand((result) =>
            {
                if (result == Recommend.RecommendResult.none)
                    callback(RecommendResult.NONE);
                else if (result == Recommend.RecommendResult.show)
                    callback(RecommendResult.DISPLAY);
                else if (result == Recommend.RecommendResult.click)
                    callback(RecommendResult.CLICK);
                else if (result == Recommend.RecommendResult.close)
                    callback(RecommendResult.DISMISS);
            });
        }
    }
}
