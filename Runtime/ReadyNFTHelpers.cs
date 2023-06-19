using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class ReadyNFTHelpers
{
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

    // overloading: the nature of the function changes based on the parameters passed
    public async Task DownloadSpriteImagesAsync(List<ReadyNFTSpriteObject> sprites)
    {
        foreach (ReadyNFTSpriteObject sprite in sprites)
        {
            await DownloadSpriteImagesAsync(sprite);
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
}
