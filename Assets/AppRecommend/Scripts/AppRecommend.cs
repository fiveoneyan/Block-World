using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Recommend
{
    public class AppRecommend : MonoBehaviour
    {
        public static AppRecommend Instance;
        public string appKey;
        public Platform platform = Platform.ios;
        public bool showInUnity = false;

        public Action eventNone;
        public Action eventShow;
        public Action eventClose;
        public Action eventClick;

        private RecommendData recommendData;
        private AppData appData;
        private bool isLoaded = false;

        private List<string> fileNames = new List<string>();
        private bool isShow = false;
        private bool isClick = false;
        private bool hasRequest = false;

        private UIRecommend uiRecommend;

        private Action<RecommendResult> _callback;
        private int recommendIndex = 0;
        private int showTimes = 0;

        void Awake()
        {
            Instance = this;
            isLoaded = false;

            DontDestroyOnLoad(this);

            uiRecommend = GetComponentInChildren<UIRecommend>();
            PlayerPrefs.SetInt("RecommendTimes", 0);
            showTimes = PlayerPrefs.GetInt("RecommendTimes");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                platform = Platform.ios;
            else if (Application.platform == RuntimePlatform.Android)
                platform = Platform.android;

#if !UNITY_EDITOR
            StartCoroutine(GetTexture());
#endif

#if UNITY_EDITOR
            if (showInUnity)
            {
                StartCoroutine(GetTexture());
            }
#endif
        }

        public void ShowRecommand(Action<RecommendResult> callback)
        {
            if (isClick)
            {
                uiRecommend.CloseRecommend();
                isClick = false;
                return;
            }

            if (isShow || PlayerPrefs.GetInt("Recommand", 0) == 0)
            {
                PlayerPrefs.SetInt("Recommand", 1);
                return;
            }

            this._callback = callback;

            if (isLoaded)
            {
                recommendIndex = 0;
                if (showTimes / 5.0f >= 1)
                {
                    recommendIndex = showTimes / 5;
                    if (recommendIndex / fileNames.Count >= 1)
                        recommendIndex -= (recommendIndex / fileNames.Count) * fileNames.Count;
                }
                showTimes++;
                eventShow?.Invoke();
                uiRecommend.Init(GetTexture(GetFilePath(fileNames[recommendIndex] + ".jpg", "Cahce")), appData.config[recommendIndex].click_url, callback, RecommendCallback);
                isShow = true;
                hasRequest = false;
                //if (PlayerPrefs.GetInt("Recommend" + fileNames[recommendIndex], 0) == 0)
                //{
                //    uiRecommend.Init(GetTexture(GetFilePath(fileNames[recommendIndex] + ".jpg", "Cahce")), appData.config[recommendIndex].click_url, callback, RecommendCallback);
                //    isShow = true;
                //    hasRequest = false;
                //    return;
                //}
                // callback(RecommendResult.none);
            }
            else
            {
                eventNone?.Invoke();
                callback(RecommendResult.none);
                hasRequest = true;
            }
        }


        void RecommendCallback(RecommendResult result)
        {
            if (result == RecommendResult.close)
            {
                isShow = false;
                eventClose?.Invoke();
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            else
            {
                if (result == RecommendResult.click)
                {
                    isClick = true;
                    eventClick?.Invoke();
                    showTimes += 5 - showTimes % 5;
                    //PlayerPrefs.SetInt("Recommend" + fileNames[recommendIndex], 1);
                    //fileNames.Remove(fileNames[recommendIndex]);
                }
                StartCoroutine(Post(result));
            }
        }

        IEnumerator GetTexture()
        {
            if (!Util.HasNetwork())
            {
                yield return new WaitForSeconds(60);
                StartCoroutine(GetTexture());
            }
            else
            {
                string _url = "https://backupserver.club/referral/list?appkey=" + appKey + "&p=" + platform.ToString() + "&uuid=" + SystemInfo.deviceUniqueIdentifier;
                UnityWebRequest unityWeb = UnityWebRequest.Get(_url);
                yield return unityWeb.SendWebRequest();
                if (!unityWeb.isNetworkError && !unityWeb.isHttpError)
                {
                    recommendData = JsonUtility.FromJson<RecommendData>(unityWeb.downloadHandler.text);
                    if (recommendData.status == 20000)
                    {
                        appData = recommendData.data;
                        if (appData.status == 0)
                        {
                            Debug.LogError(recommendData.msg);
                        }
                        else
                        {
                            string textureName = "";
                            DeviceModel device = Util.GetDevice();
                            switch (device)
                            {
                                case DeviceModel.IPad:
                                    textureName = appData.bg[0];
                                    break;
                                case DeviceModel.IPhoneSE:
                                    textureName = appData.bg[1];
                                    break;
                                case DeviceModel.IPhoneX:
                                    textureName = appData.bg[2];
                                    break;
                            }
                            for (int i = 0; i < appData.config.Count; i++)
                            {
                                string url = appData.url + appData.config[i].id + "/" + textureName;
                                string curName = appData.config[i].id + "_" + device.ToString();
                                //if (PlayerPrefs.GetInt("Recommend" + curName, 0) == 0)
                                fileNames.Add(curName);
                                string filePath = GetFilePath(curName + ".jpg", "Cahce");
                                if (!CheckIsExitFile(filePath))
                                {
                                    yield return DownloadTexture(url, filePath);
                                }
                                else
                                {
                                    Debug.Log("图片" + curName + "已存在，无需再次下载！");
                                }
                            }
                            isLoaded = true;
                            Debug.Log("初始化完成！");
                            if (hasRequest)
                            {
                                ShowRecommand(_callback);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError(recommendData.msg);
                    }
                }
                else
                {
                    Debug.LogError("数据获取失败:" + unityWeb.error);
                }
            }
        }

        public string GetFilePath(string filename, string rootName)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                path = Path.Combine(path, "Documents");
                path = Path.Combine(path, rootName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return Path.Combine(path, filename);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                string path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                path = Path.Combine(path, rootName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return Path.Combine(path, filename);
            }
            else
            {
                string path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                path = Path.Combine(path, rootName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return Path.Combine(path, filename);
            }
        }


        /// <summary>
        /// 获取到本地的图片
        /// </summary>
        /// <param name="path"></param>
        public Texture2D GetTexture(string path)
        {
            var pathName = path;
            var bytes = ReadFile(pathName);
            int width = Screen.width;
            int height = Screen.height;
            var texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            return texture;
        }

        public byte[] ReadFile(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            var binary = new byte[fs.Length];
            fs.Read(binary, 0, binary.Length);
            fs.Close();
            return binary;
        }


        bool CheckIsExitFile(string filePath)
        {
            if (File.Exists(filePath))
                return true;
            else
                return false;
        }

        IEnumerator DownloadTexture(string url, string filePath)
        {
            Debug.Log("开始下载图片：" + url);
            UnityWebRequest unityWeb = UnityWebRequest.Get(url);
            yield return unityWeb.SendWebRequest();
            if (!unityWeb.isNetworkError && !unityWeb.isHttpError)
            {
                byte[] bytes = unityWeb.downloadHandler.data;
                FileStream file = new FileStream(filePath, FileMode.Create);
                file.Write(bytes, 0, bytes.Length);
                file.Close();
                Debug.Log("图片下载成功：" + url);
            }
            else
            {
                Debug.LogError("图片下载失败:" + unityWeb.error);
            }
        }

        IEnumerator Post(RecommendResult result)
        {
            WWWForm form = new WWWForm();
            form.AddField("appkey", appKey);
            form.AddField("rkey", appData.config[0].id);
            form.AddField("p", platform.ToString());
            form.AddField("uuid", SystemInfo.deviceUniqueIdentifier);
            form.AddField("t", result.ToString());
            UnityWebRequest unityWeb = UnityWebRequest.Post("https://backupserver.club/referral/statistics", form);
            yield return unityWeb.SendWebRequest();
            if (!unityWeb.isNetworkError && !unityWeb.isHttpError)
            {
                Debug.Log("请求成功:" + unityWeb.downloadHandler.text);
            }
            else
            {
                Debug.LogError("请求失败:" + unityWeb.error);
            }
        }
    }

    public enum Platform
    {
        ios,
        android,
    }

    public enum RecommendResult
    {
        none = 0,
        show,
        close,
        click,
    }

    public enum PostResult
    {
        success,
        faild
    }


    [Serializable]
    public class RecommendData
    {
        public string msg;
        public int status;
        public AppData data;
    }

    [Serializable]
    public class AppData
    {
        public string url;
        public int status;
        public List<string> bg;
        public List<AppConfig> config;
    }

    [Serializable]
    public class AppConfig
    {
        public string name;
        public string click_url;
        public string id;
    }
}