using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class ReadyNFTDownloadReport
{
    [Preserve]
    [JsonProperty("percent")]
    public float percent { get; set; }

    [Preserve]
    [JsonProperty("total")]
    public int total { get; set; }

    [Preserve]
    [JsonProperty("current")]
    public int current { get; set; }
    public ReadyNFTDownloadReport(float percent, int total, int current)
    {
        this.percent = percent;
        this.total = total;
        this.current = current;
    }
}