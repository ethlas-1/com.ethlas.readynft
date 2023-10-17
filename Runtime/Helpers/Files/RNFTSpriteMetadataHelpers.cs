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


public class RNFTSpriteMetadataHelpers
{
    private int currentProgress = 0;

    public string SPRITE_METADATA_JSON = "sprite_metadata.json";
    public string GetReadyNFTImageDirectory()
    {
        string path = Application.persistentDataPath + "/ReadyNFT/Images/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        return path;
    }

    public string GetReadyNFTMetadataDirectory()
    {
        string path = Application.persistentDataPath + "/ReadyNFT/Metadata/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        return path;
    }

    public async Task CreateSpriteMetadataFileAsync(List<ReadyNFTSpriteObject> sprites)
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        // check if the key already exists, if it does, just update it
        // else add in a new key
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            if (dict.ContainsKey(sprite.contract))
            {
                dict[sprite.contract] = sprite.version;
            }
            else
            {
                dict.Add(sprite.contract, sprite.version);
            }
        }
        string json = JsonConvert.SerializeObject(dict);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<Dictionary<string, int>> ReadSpriteMetadataFileAsync()
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        if (!File.Exists(path))
        {
            return new Dictionary<string, int>();
        }
        string json = await File.ReadAllTextAsync(path);
        Dictionary<string, int> dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        return dict;
    }

    public async Task UpdateSpriteMetadataFileAsync(ReadyNFTSpriteObject sprite)
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        Dictionary<string, int> dict = await ReadSpriteMetadataFileAsync();
        if (dict.ContainsKey(sprite.contract))
        {
            dict[sprite.contract] = sprite.version;
        }
        else
        {
            dict.Add(sprite.contract, sprite.version);
        }
        string json = JsonConvert.SerializeObject(dict);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<List<string>> GetCollectionsToUpdate(List<ReadyNFTSpriteObject> sprites)
    {
        Dictionary<string, int> metadataDict = await ReadSpriteMetadataFileAsync();
        List<string> keysToUpdate = new List<string>();
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            string contract = sprite.contract;
            int version = sprite.version;
            if (metadataDict.ContainsKey(contract))
            {
                int metadataVersion = metadataDict[contract];
                if (metadataVersion < version)
                {
                    keysToUpdate.Add(contract);
                }
            }
            else
            {
                keysToUpdate.Add(contract);
            }
        }
        return keysToUpdate;
    }

    // overloading: the nature of the function changes based on the parameters passed
    public async Task DownloadSpriteImagesAsync(List<ReadyNFTSpriteObject> sprites, IProgress<ReadyNFTDownloadReport> progress = null)
    {
        // fetch the list of contracts that need to be updated
        List<string> updatedCollections = await GetCollectionsToUpdate(sprites);

        int total = updatedCollections.Count;
        currentProgress = 0;

        if (total == 0)
        {
            if (progress != null)
            {
                ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(100f, 1, 1);
                progress.Report(report);
            }
            return;
        }

        List<Task> waitTasksList = new List<Task>();

        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            if (!updatedCollections.Contains(sprite.contract))
            {
                ++currentProgress;
                // no decimal places in percentage
                float newPercent = (float)currentProgress / (float)total * 100f;
                if (progress != null)
                {
                    ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(newPercent, total, currentProgress);
                    progress.Report(report);
                }
                continue;
            }

            waitTasksList.Add(DownloadSpriteImagesAsync(sprite, total, progress));
            
        }

        await Task.WhenAll(waitTasksList);

        // update the metadata file
        await CreateSpriteMetadataFileAsync(sprites);
    }

    public async Task DownloadSpriteImagesAsync(ReadyNFTSpriteObject sprite, int total, IProgress<ReadyNFTDownloadReport> progress = null)
    {
        List<Task> waitTasksList = new List<Task>();
        // sprite.images is a Dictionary<string, string> of key and image url
        // loop thtough images and download them
        foreach (KeyValuePair<string, string> entry in sprite.images)
        {
            string key = entry.Key;
            string imageUrl = entry.Value;
            string savePath = GetReadyNFTImageDirectory() + sprite.contract + "_" + key + ".png";
            waitTasksList.Add(DownloadImageAsync(imageUrl, savePath));
        }

        await Task.WhenAll(waitTasksList);

        ++currentProgress;
        // no decimal places in percentage
        float percent = (float)currentProgress / (float)total * 100f;
        if (progress != null)
        {
            ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(percent, total, currentProgress);
            progress.Report(report);
        }
    }

    public async Task DownloadImageAsync(string imageUrl, string savePath)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            // Send the request and wait for a response
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Image download failed: {webRequest.error}");
                return;
            }

            // Get the downloaded texture
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

            // Convert the texture to a byte array (PNG format)
            byte[] bytes = texture.EncodeToPNG();

            // Save the image to disk
            await File.WriteAllBytesAsync(savePath, bytes);
            Debug.Log($"Image downloaded and saved to: {savePath}");
        }
    }
    
    public void DeleteSpritesMetaData()
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }

    public void DeleteAllImages()
    {
        string path = GetReadyNFTImageDirectory();
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }

    public int GetNumberOfImages()
    {
        string path = GetReadyNFTImageDirectory();
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            return files.Length;
        }
        return 0;
    }
}
