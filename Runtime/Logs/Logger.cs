using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class Logger
{
    const string AMPLITUDE_KEY = "KEY_HERE";
    const string AMPLITUDE_URL = "https://api2.amplitude.com/2/httpapi";

    // function to get the event 
    private static RNFTAmplitudeEvent GetEvent(string eventType, Dictionary<string, string> eventProperties)
    {
        RNFTMetaDataHelpers metaDataHelpers = new RNFTMetaDataHelpers();
        ReadyNFTMetaData metadata = metaDataHelpers.GetMetaData();
        RNFTAmplitudeEvent amplitudeEvent = metaDataHelpers.ConvertMetadataToAmplitudeEvent(metadata);
        amplitudeEvent.event_properties = eventProperties;
        amplitudeEvent.event_type = eventType;
        return amplitudeEvent;
    }

    // function to send the event to amplitude
    private static async void LogEventToAmplitude(RNFTAmplitudeEvent amplitudeEvent)
    {
        HttpClient client = new HttpClient();

        // set the headers
        client.DefaultRequestHeaders.Add("Content-Type", "application/json");

        // set the body
        Dictionary<string, object> body = new Dictionary<string, object>();
        body.Add("api_key", AMPLITUDE_KEY);
        body.Add("events", new RNFTAmplitudeEvent[] { amplitudeEvent });

        // serialize the body
        string jsonBody = JsonConvert.SerializeObject(body);

        // make the request
        HttpResponseMessage response = await client.PostAsync(AMPLITUDE_URL, new StringContent(jsonBody));

        // if status code is not 200, log the event to console
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Debug.Log("Amplitude event failed to send");
        }
    }
}

