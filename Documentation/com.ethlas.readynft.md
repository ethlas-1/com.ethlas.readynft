
# ReadyNFT Class

Provides methods for interacting with the ReadyNFT API.

## Methods

### `Init(string _apiKey, string _gameId)`

Initialization method to store the API key and game ID.
> The init method must be called before any other method. Else the other methods will return empty responses.

| Parameter  | Type   | Description            |
| ---------- | ------ | ---------------------- |
| `_apiKey`  | string | The API key to be set. |
| `_gameId`  | string | The game ID to be set. |

### `Task<List<ReadyNFTSpriteObject>> FetchSpritesAsync()`

Asynchronously fetches the sprite objects from the ReadyNFT API.

| Return Type                      | Description                                           |
| -------------------------------- | ----------------------------------------------------- |
| `Task<List<ReadyNFTSpriteObject>>` | A task that represents the asynchronous fetch operation. |

A sample response from the function call will be as follows:
```
[
  {
    "gameId": "123",
    "nftId": "456",
    "nftName": "MyNFT",
    "contract": "0x123abc",
    "images": {
      "thumbnail": "https://example.com/nft/thumbnail.jpg",
      "large": "https://example.com/nft/large.jpg"
    },
    "stats": {
      "power": 100,
      "speed": 75
    }
  },
  {
    "gameId": "789",
    "nftId": "012",
    "nftName": "AnotherNFT",
    "contract": "0x456def",
    "images": {
      "thumbnail": "https://example.com/nft2/thumbnail.jpg",
      "large": "https://example.com/nft2/large.jpg"
    },
    "stats": {
      "power": 85,
      "speed": 90
    }
  }
]

```


### `Task<List<ReadyNFTOwnedNFTObject>> FetchOwnedNFTsAsync(string email)`

Asynchronously fetches the owned NFT objects associated with the provided email from the ReadyNFT API.

| Parameter  | Type   | Description        |
| ---------- | ------ | ------------------ |
| `email`    | string | The user's email.  |

| Return Type                         | Description                                           |
| ----------------------------------- | ----------------------------------------------------- |
| `Task<List<ReadyNFTOwnedNFTObject>>` | A task that represents the asynchronous fetch operation. |

A sample response from the function call will be as follows:

```
[
  {
    "contractAddress": "0x123abc",
    "collectionName": "MyNFTCollection",
    "collectionSymbol": "MNC",
    "tokenId": "987654321",
    "tokenType": "ERC721"
  },
  {
    "contractAddress": "0x456def",
    "collectionName": "AnotherNFTCollection",
    "collectionSymbol": "ANC",
    "tokenId": "123456789",
    "tokenType": "ERC1155"
  }
]

```

### `string GetApiKey()`

Gets the API key.

| Return Type | Description                            |
| ----------- | -------------------------------------- |
| `string`    | The stored API key.                     |

### `string GetGameId()`

Gets the Game ID.

| Return Type | Description                            |
| ----------- | -------------------------------------- |
| `string`    | The stored Game ID.                     |

Please note that the private variables and unnecessary classes are excluded from the documentation.

# Types

# ReadyNFTSpriteObject Class

Represents a sprite object retrieved from the ReadyNFT API.

## Properties

| Name              | Type                         | Description                                   |
| ----------------- | ---------------------------- | --------------------------------------------- |
| `gameId`          | string                       | The ID of the game.                           |
| `nftId`           | string                       | The ID of the NFT.                            |
| `nftName`         | string                       | The name of the NFT.                          |
| `contract`        | string                       | The contract of the NFT.                      |
| `images`          | Dictionary<string, string>   | URLs of the sprite images.                    |
| `stats`           | Dictionary<string, int>      | Statistics of the sprite.                     |

# ReadyNFTOwnedNFTObject Class

Represents an owned NFT object retrieved from the ReadyNFT API.

## Properties

| Name                | Type   | Description                             |
| ------------------- | ------ | --------------------------------------- |
| `contractAddress`   | string | Contract address of the owned NFT.       |
| `collectionName`    | string | Name of the collection the NFT belongs to. |
| `collectionSymbol`  | string | Symbol of the collection the NFT belongs to. |
| `tokenId`           | string | ID of the NFT.                           |
| `tokenType`         | string | Type of the NFT.                         |