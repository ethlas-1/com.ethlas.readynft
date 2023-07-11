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


public class ReadyNFTFileHelpers
{
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
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            dict.Add(sprite.contract, sprite.version);
        }
        string json = JsonConvert.SerializeObject(dict);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<Dictionary<string, string>> ReadSpriteMetadataFileAsync()
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        if (!File.Exists(path))
        {
            return new Dictionary<string, string>();
        }
        string json = await File.ReadAllTextAsync(path);
        Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        return dict;
    }

    public async Task UpdateSpriteMetadataFileAsync(ReadyNFTSpriteObject sprite)
    {
        string path = GetReadyNFTMetadataDirectory() + SPRITE_METADATA_JSON;
        Dictionary<string, string> dict = await ReadSpriteMetadataFileAsync();
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
        Dictionary<string, string> metadataDict = await ReadSpriteMetadataFileAsync();
        List<string> keysToUpdate = new List<string>();
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            string contract = sprite.contract;
            string version = sprite.version;
            if (metadataDict.ContainsKey(contract))
            {
                string metadataVersion = metadataDict[contract];
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
        int total = sprites.Count;
        int current = 0;
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            await DownloadSpriteImagesAsync(sprite);
            current++;
            // no decimal places in percentage
            float percent = (float)current / (float)total * 100f;
            if (progress != null)
            {
                ReadyNFTDownloadReport report = new ReadyNFTDownloadReport(percent, total, current);
                progress.Report(report);
            }
        }
    }

    public async Task DownloadSpriteImagesAsync(ReadyNFTSpriteObject sprite)
    {
        // sprite.images is a Dictionary<string, string> of key and image url
        // loop thtough images and download them
        foreach (KeyValuePair<string, string> entry in sprite.images)
        {
            string key = entry.Key;
            string imageUrl = entry.Value;
            string savePath = GetReadyNFTImageDirectory() + sprite.contract + "_" + key + ".png";
            await DownloadImageAsync(imageUrl, savePath);
        }
    }

    public async Task DownloadImageAsync(string imageUrl, string savePath)
    {
        // Check if file exists
        if (File.Exists(savePath))
        {
            Debug.Log($"Image already exists: {savePath}");
            return;
        }

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
            File.WriteAllBytes(savePath, bytes);
            Debug.Log($"Image downloaded and saved to: {savePath}");
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