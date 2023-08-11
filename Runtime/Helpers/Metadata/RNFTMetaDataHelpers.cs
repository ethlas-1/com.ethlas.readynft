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

public class RNFTMetaDataHelpers
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

    private string GetAppVersion()
    {
        string version = "default";
        string _version = Application.version;
        if (_version != null && _version != "")
        {
            version = _version;
        }

        return version;
    }

    private string GetBundleIdentifier()
    {
        string identifier = "default";
        string _identifier = Application.identifier;
        if (_identifier != null && _identifier != "")
        {
            identifier = _identifier;
        }

        return identifier;
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
        string appVersion = GetAppVersion();
        string bundleIdentifier = GetBundleIdentifier();

        ReadyNFTMetaData metaData = new ReadyNFTMetaData(country, deviceId, deviceBrand, deviceManufacturer, deviceModel, osName, osVersion, appVersion, bundleIdentifier);
        return metaData;
    }

    public RNFTAmplitudeEvent ConvertMetadataToAmplitudeEvent(ReadyNFTMetaData metadata)
    {
        RNFTAmplitudeEvent amplitudeEvent = new RNFTAmplitudeEvent();
        amplitudeEvent.user_id = metadata.deviceId;
        amplitudeEvent.country = metadata.country;
        amplitudeEvent.device_id = metadata.deviceId;
        amplitudeEvent.device_brand = metadata.deviceBrand;
        amplitudeEvent.device_manufacturer = metadata.deviceManufacturer;
        amplitudeEvent.device_model = metadata.deviceModel;
        amplitudeEvent.os_name = metadata.osName;
        amplitudeEvent.os_version = metadata.osVersion;
        amplitudeEvent.app_version = metadata.appVersion;
        amplitudeEvent.bundle_identifier = metadata.bundleIdentifier;
        return amplitudeEvent;
    }
}