using System.Collections.Generic;

public class ReadyNFTSpriteObject
{
    public string gameId { get; set; }
    public Dictionary<string, string> images { get; set; }
    public Dictionary<string, int> stats { get; set; }
}

public class FetchSpritesResponse
{
    public string message { get; set; }
    public FetchSpritesData data { get; set; }
}

public class FetchSpritesData
{
    public List<ReadyNFTSpriteObject> sprites { get; set; }
}
