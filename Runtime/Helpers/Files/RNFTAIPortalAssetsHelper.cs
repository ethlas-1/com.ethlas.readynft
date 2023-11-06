using System.Net;
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

public class RNFTAIPortalAssetsHelpers
{
    public string AI_PORTAL_METADATA_JSON = "ai_portal_metadata.json";
    int currentProgress = 0; // it is a class variable so that it is accessible by the different methods

    public string GetRNFTAIPortalAssetsDirectory()
    {
        string path = Application.persistentDataPath + "/RNFTAIPortal/Assets/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        return path;
    }

    public string GetRNFTAIPortalMetadataDirectory()
    {
        string path = Application.persistentDataPath + "/RNFTAIPortal/Metadata/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        return path;
    }

    public async Task CreateAIPortalMetadataFileAsync(List<RNFTAIPortalAssetObject> assets)
    {
        string path = GetRNFTAIPortalMetadataDirectory() + AI_PORTAL_METADATA_JSON;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        // check if the key already exists, if it does, just update it
        // else add in a new key

        foreach (RNFTAIPortalAssetObject asset in assets)
        {
            if (dict.ContainsKey(asset.assetId))
            {
                dict[asset.assetId] = asset.version;
            }
            else
            {
                dict.Add(asset.assetId, asset.version);
            }
        }
    }

    public async Task<Dictionary<string, int>> ReadAIPortalMetadataFileAsync()
    {
        string path = GetRNFTAIPortalMetadataDirectory() + AI_PORTAL_METADATA_JSON;
        if (!File.Exists(path))
        {
            return new Dictionary<string, int>();
        }
        string json = await File.ReadAllTextAsync(path);
        Dictionary<string, int> dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        return dict;
    }

    public async Task UpdateAIPortalMetadataFileAsync(RNFTAIPortalAssetObject asset)
    {
        string path = GetRNFTAIPortalMetadataDirectory() + AI_PORTAL_METADATA_JSON;
        Dictionary<string, int> dict = await ReadAIPortalMetadataFileAsync();

        if (dict.ContainsKey(asset.assetId))
        {
            dict[asset.assetId] = asset.version;
        }
        else
        {
            dict.Add(asset.assetId, asset.version);
        }
        string json = JsonConvert.SerializeObject(dict);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<List<string>> GetAssetsToUpdate(List<RNFTAIPortalAssetObject> assets)
    {
        Dictionary<string, int> metadataDict = await ReadAIPortalMetadataFileAsync();
        List<string> assetsToUpdate = new List<string>();
        foreach (RNFTAIPortalAssetObject asset in assets)
        {
            string assetId = asset.assetId;
            int version = asset.version;
            if (metadataDict.ContainsKey(assetId))
            {
                int metadataVersion = metadataDict[assetId];
                if (metadataVersion < version)
                {
                    assetsToUpdate.Add(assetId);
                }
            }
            else
            {
                assetsToUpdate.Add(assetId);
            }
        }
        return assetsToUpdate;
    }

    public async Task DownloadAIPortalAssetsAsync(List<RNFTAIPortalAssetObject> assets, IProgress<ReadyNFTDownloadReport> progress = null)
    {
        // get the assets to update
        List<string> assetsToUpdate = await GetAssetsToUpdate(assets);

        int totalAssets = assetsToUpdate.Count;
        currentProgress = 0;

        if (totalAssets == 0)
        {
            if (progress != null)
            {
                ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(100f, 1, 1);
                progress.Report(report);
            }
            return;
        }

        List<Task> waitTasksList = new List<Task>();

        foreach (RNFTAIPortalAssetObject asset in assets)
        {
            if (!assetsToUpdate.Contains(asset.assetId))
            {
                ++currentProgress;
                float newPercent = (float)currentProgress / (float)totalAssets * 100f;
                if (progress != null)
                {
                    ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(newPercent, totalAssets, currentProgress);
                    progress.Report(report);
                }
                continue;
            }

            waitTasksList.Add(DownloadImageAsync(asset, totalAssets, progress));
        }

        await Task.WhenAll(waitTasksList);

        // update the metadata file
        await CreateAIPortalMetadataFileAsync(assets);
    }

    public async Task DownloadImageAsync(RNFTAIPortalAssetObject asset, int total, IProgress<ReadyNFTDownloadReport> progress = null)
    {
        string savePath = GetRNFTAIPortalAssetsDirectory() + asset.playerUuid + ".png";
        string imageUrl = asset.imageUrl;

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(asset.imageUrl))
        {
            // send the request and wait for a response 
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error downloading image: " + imageUrl + " " + webRequest.error);
                if (progress != null)
                {
                    ++currentProgress;
                    ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(0f, total, currentProgress);
                    progress.Report(report);
                }

            }

            // Get the downloaded texture
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

            // Convert the texture to a byte array (PNG format)
            byte[] bytes = texture.EncodeToPNG();

            // Save the image to disk
            await File.WriteAllBytesAsync(savePath, bytes);
            Debug.Log($"Image downloaded and saved to: {savePath}");

            if (progress != null)
            {
                ++currentProgress;
                float newPercent = (float)currentProgress / (float)total * 100f;
                ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(newPercent, total, currentProgress);
                progress.Report(report);
            }

        }
    }

    public void DeleteAIPortalMetadata()
    {
        string path = GetRNFTAIPortalMetadataDirectory() + AI_PORTAL_METADATA_JSON;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }

    public void DeleteAllAIPortalAssets()
    {
        string path = GetRNFTAIPortalAssetsDirectory();
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }

    public int GetNumberOfAIPortalAssets()
    {
        string path = GetRNFTAIPortalAssetsDirectory();
        if (!Directory.Exists(path))
        {
            return 0;
        }
        string[] files = Directory.GetFiles(path);
        return files.Length;
    }
}