using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

public static class RNFTLogger
{
    const string AMPLITUDE_KEY = "8b5a0f623da9dc7fbbe7c24712162d4e";
    const string AMPLITUDE_URL = "https://api2.amplitude.com/2/httpapi";

    // function to get the event 
    private static RNFTAmplitudeEvent GetEvent(string eventType, Dictionary<string, string> eventProperties)
    {
        RNFTMetaDataHelpers metaDataHelpers = new RNFTMetaDataHelpers();
        ReadyNFTMetaData metadata = metaDataHelpers.GetMetaData();
        RNFTAmplitudeEvent amplitudeEvent = metaDataHelpers.ConvertMetadataToAmplitudeEvent(metadata);
        amplitudeEvent.event_properties = eventProperties;
        amplitudeEvent.event_type = eventType;

        // add the app version and the bundle identifier to the event properties
        amplitudeEvent.event_properties.Add("app_version", metadata.appVersion);
        amplitudeEvent.event_properties.Add("bundle_identifier", metadata.bundleIdentifier);
        return amplitudeEvent;
    }

    // function to send the event to amplitude
    private static async void LogEventToAmplitude(RNFTAmplitudeEvent amplitudeEvent)
{
    HttpClient client = new HttpClient();

    // set the body
    Dictionary<string, object> body = new Dictionary<string, object>();
    body.Add("api_key", AMPLITUDE_KEY);
    body.Add("events", new RNFTAmplitudeEvent[] { amplitudeEvent });

    // serialize the body
    string jsonBody = JsonConvert.SerializeObject(body);

    // Create StringContent with headers
    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

    // make the request
    HttpResponseMessage response = await client.PostAsync(AMPLITUDE_URL, content);

    // if status code is not 200, log the event to console
    if (response.StatusCode != System.Net.HttpStatusCode.OK)
    {
        Debug.Log("Amplitude event failed to send");
    }
}


    // function to log the event
    public static void LogEvent(string eventType, Dictionary<string, string> eventProperties = null)
    {
#if UNITY_EDITOR
    Debug.Log("Amplitude event logged: " + eventType);
    return;
#endif

        if (eventProperties == null)
        {
            eventProperties = new Dictionary<string, string>();
        }

        RNFTAmplitudeEvent amplitudeEvent = GetEvent(eventType, eventProperties);
        LogEventToAmplitude(amplitudeEvent);
    }
}

