using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ReadyNFTMetaDataHelpers
{
    public string GetDeviceId()
    {
        string key = "@ReadyNFTDeviceID";
        string readyNFTDeviceID = PlayerPrefs.GetString(key);

        if (readyNFTDeviceID != null && readyNFTDeviceID != "")
        {
            return readyNFTDeviceID;
        }
        else
        {
            Guid myGuid = Guid.NewGuid();
            string deviceId = myGuid.ToString();
            PlayerPrefs.SetString(key, deviceId);
            PlayerPrefs.Save();
            return deviceId;
        }
    }

    private string GetDeviceManufacturer()
    {
        {
            string manufacturer = "default";

#if UNITY_IOS
        manufacturer = "Apple";
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build");
        manufacturer = buildClass.GetStatic<string>("MANUFACTURER");
#endif
            return manufacturer;
        }
    }

    private string GetDeviceBrand()
    {
        string brand = "default";

#if UNITY_IOS
        brand = "Apple";
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build");
        brand = buildClass.GetStatic<string>("BRAND");
#endif

        return brand;
    }

    private string GetDeviceModel()
    {
        string model = "default";

#if UNITY_IOS
        model = UnityEngine.iOS.Device.generation.ToString();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build");
        model = buildClass.GetStatic<string>("MODEL");
#endif

        return model;
    }

    private string GetOperatingSystemName()
    {
        string os = "default";

#if UNITY_IOS
        os = "iOS";
#elif UNITY_ANDROID && !UNITY_EDITOR
        os = "Android";
#endif

        return os;
    }

    private string GetOperatingSystemVersion()
    {
        string version = "default";

#if UNITY_IOS
        version = UnityEngine.iOS.Device.systemVersion;
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build$VERSION");
        version = buildClass.GetStatic<string>("RELEASE");
#endif

        return version;
    }

    public ReadyNFTMetaData GetMetaData()
    {
        string country = RegionInfo.CurrentRegion.EnglishName;
        string deviceId = GetDeviceId();
        string deviceBrand = GetDeviceBrand();
        string deviceManufacturer = GetDeviceManufacturer();
        string deviceModel = GetDeviceModel();
        string osName = GetOperatingSystemName();
        string osVersion = GetOperatingSystemVersion();

        ReadyNFTMetaData metaData = new ReadyNFTMetaData(country, deviceId, deviceBrand, deviceManufacturer, deviceModel, osName, osVersion);
        return metaData;
    }

}