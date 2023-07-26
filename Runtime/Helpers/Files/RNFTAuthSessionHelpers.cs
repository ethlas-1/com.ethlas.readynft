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

public class RNFTAuthSessionHelpers
{
    public string AUTH_SESSION_JSON = "auth_session_data.json";

    public string GetRNFTAuthDataDirectory()
    {
        string path = Application.persistentDataPath + "/ReadyNFT/Auth/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("[RNFT] Directory created: " + path);
        }
        return path;
    }

    public async Task CreateAuthDataFile(RNFTAuthTokensType tokens)
    {
        string path = GetRNFTAuthDataDirectory() + AUTH_SESSION_JSON;
        string json = JsonConvert.SerializeObject(tokens);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<RNFTAuthTokensType> ReadAuthDataFileAsync()
    {
        string path = GetRNFTAuthDataDirectory() + AUTH_SESSION_JSON;
        if (!File.Exists(path))
        {
            return new RNFTAuthTokensType();
        }
        string json = await File.ReadAllTextAsync(path);
        RNFTAuthTokensType tokens = JsonConvert.DeserializeObject<RNFTAuthTokensType>(json);
        return tokens;
    }

    public async void UpdateSpriteMetadataFileAsync(RNFTAuthTokensType tokens)
    {
        DeleteAuthDataFile();
        await CreateAuthDataFile(tokens);
    }
    
    public void DeleteAuthDataFile()
    {
        string path = GetRNFTAuthDataDirectory() + AUTH_SESSION_JSON;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }
}