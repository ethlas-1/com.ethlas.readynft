using System.Collections.Generic;
using System;


// sprites start
[Serializable]
public class ReadyNFTSpriteObject
{
    public string gameId { get; set; }
    public string nftId { get; set; }
    public string nftName { get; set; }
    public string contract { get; set; }
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
// sprites end

// ownedNFTs start
[Serializable]
public class ReadyNFTOwnedNFTObject
{
    public string contractAddress { get; set; }
    public string collectionName { get; set; }
    public string collectionSymbol { get; set; }
    public string tokenId { get; set; }
    public string tokenType { get; set; }
}

public class FetchOwnedNFTsResponse
{
    public string message { get; set; }
    public FetchOwnedNFTsData data { get; set; }
}

public class FetchOwnedNFTsData
{
    public List<ReadyNFTOwnedNFTObject> nfts { get; set; }
}
