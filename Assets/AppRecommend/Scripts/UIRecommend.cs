using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Recommend
{
    public class UIRecommend : MonoBehaviour
    {
        public RawImage img;

        private Action<RecommendResult> Getcallback;
        private Action<RecommendResult> PostCallback;
        private string url;

        private bool _handleAble = true;

        public void Init(Texture tex, string url, Action<RecommendResult> Getcallback, Action<RecommendResult> PostCallback)
        {
            img.texture = tex;
            _handleAble = true;
            img.gameObject.SetActive(true);
            this.Getcallback = Getcallback;
            this.PostCallback = PostCallback;
            this.url = url;
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                GetComponent<CanvasScaler>().referenceResolution = new Vector2(1136, 640);
                GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                img.transform.localEulerAngles = new Vector3(0, 0, -90);
                img.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                img.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                float count = 0.001f;
                DOTween.To(() => count, x => count = x, 0, 0.001f).OnComplete(() =>
                         {
                             img.rectTransform.sizeDelta = new Vector2(Screen.height, Screen.width) / transform.localScale.x;
                         });
            }
            img.transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
            Getcallback?.Invoke(RecommendResult.show);
            PostCallback?.Invoke(RecommendResult.show);
        }

        public void CloseRecommend()
        {
            if (!_handleAble)
                return;

            _handleAble = false;

            img.transform.DOLocalMoveY(-Screen.height, 0.2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                img.gameObject.SetActive(false);
                Getcallback?.Invoke(RecommendResult.close);
                PostCallback?.Invoke(RecommendResult.close);
            });
        }

        public void OnRecommendClick()
        {
            if (!_handleAble)
                return;

            Application.OpenURL(url);
            Getcallback?.Invoke(RecommendResult.click);
            PostCallback?.Invoke(RecommendResult.click);
        }
    }
}
