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

public static class RNFTAuthSessionHelpers
{
    public static string AUTH_SESSION_JSON = "auth_session_data.json";

    public static string GetRNFTAuthDataDirectory()
    {
        string path = Application.persistentDataPath + "/ReadyNFT/Auth/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("[RNFT] Directory created: " + path);
        }
        return path;
    }

    public static async Task CreateAuthDataFile(RNFTAuthTokensType tokens)
    {
        string path = GetRNFTAuthDataDirectory() + AUTH_SESSION_JSON;
        string json = JsonConvert.SerializeObject(tokens);
        await File.WriteAllTextAsync(path, json);
        Debug.Log("[RNFT] Created Auth Session JSON");
    }

    public static async Task<RNFTAuthTokensType> ReadAuthDataFileAsync()
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

    public static async void UpdateAuthSessionData(RNFTAuthTokensType tokens)
    {
        DeleteAuthDataFile();
        await CreateAuthDataFile(tokens);
    }
    
    public static void DeleteAuthDataFile()
    {
        string path = GetRNFTAuthDataDirectory() + AUTH_SESSION_JSON;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Directory deleted: " + path);
        }
    }
}