using UnityEngine;

namespace Recommend
{
    public class Util
    {
        public static DeviceModel GetDevice()
        {
            DeviceModel deviceModel = DeviceModel.IPhoneSE;
            float result = (Screen.width * 1.0f) / (Screen.height * 1.0f);
            if (Mathf.Approximately(result, 640.0f / 1136.0f) || Mathf.Approximately(result, 1136.0f / 640.0f))
                deviceModel = DeviceModel.IPhoneSE;
            else if (Mathf.Approximately(result, 852.0f / 1386.0f) || Mathf.Approximately(result, 1386.0f / 852.0f))
                deviceModel = DeviceModel.IPhoneSE;
            else if (Mathf.Approximately(result, 1125.0f / 2436.0f) || Mathf.Approximately(result, 2436.0f / 1125.0f))
                deviceModel = DeviceModel.IPhoneX;
            else if (Mathf.Approximately(result, 828.0f / 1792.0f) || Mathf.Approximately(result, 1792.0f / 828.0f))
                deviceModel = DeviceModel.IPhoneX;
            else if (Mathf.Approximately(result, 1242.0f / 2688.0f) || Mathf.Approximately(result, 2688.0f / 1242.0f))
                deviceModel = DeviceModel.IPhoneX;
            else if (Mathf.Approximately(result, 1536.0f / 2048.0f) || Mathf.Approximately(result, 2048.0f / 1536.0f))
                deviceModel = DeviceModel.IPad;
            else if (Mathf.Approximately(result, 2048.0f / 2732.0f) || Mathf.Approximately(result, 2732.0f / 2048.0f))
                deviceModel = DeviceModel.IPad;
            else if (Mathf.Approximately(result, 1668.0f / 2224.0f) || Mathf.Approximately(result, 2224.0f / 1668.0f))
                deviceModel = DeviceModel.IPad;

            return deviceModel;
        }

        public static bool HasNetwork()
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                return true;
            else
                return false;
        }
    }

    public enum DeviceModel
    {
        IPhoneSE,
        IPhoneX,
        IPad,
    }
}