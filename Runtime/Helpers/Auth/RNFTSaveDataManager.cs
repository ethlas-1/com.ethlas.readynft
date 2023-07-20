// Used to save scripts to json that implement ISaveable
// based on https://github.com/UnityTechnologies/UniteNow20-Persistent-Data

public interface ISaveable
{
    string ToJson();
    void LoadFromJson(string a_Json);
    string FileNameToUseForData();
}


public static class RNFTSaveDataManager
{
    public static void SaveJsonData(ISaveable saveable)
    {
        if (RNFTFileManager.WriteToFile(saveable.FileNameToUseForData(), saveable.ToJson()))
        {
            // Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(ISaveable saveable)
    {
        if (RNFTFileManager.LoadFromFile(saveable.FileNameToUseForData(), out var json))
        {
            saveable.LoadFromJson(json);
            // Debug.Log("Load complete");
        }
    }
}