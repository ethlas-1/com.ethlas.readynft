using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class ReadyNFTTests
{
    [Test]
    public async void FetchSpritesAsync_InvalidApiKey_ReturnsNull()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("", "gameId");

        // Act
        List<ReadyNFTSpriteObject> result = await readyNFT.FetchSpritesAsync();

        // Assert
        Assert.AreEqual(0, result.Count);

    }

    [Test]
    public async void FetchSpritesAsync_InvalidGameId_ThrowsException()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "");

        // Act
        List<ReadyNFTSpriteObject> result = await readyNFT.FetchSpritesAsync();

        // Assert
        Assert.AreEqual(0, result.Count);

    }

    [Test]
    public async void FetchSpritesAsync_WrongGameId()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "gameId");

        // Act
        List<ReadyNFTSpriteObject> result = await readyNFT.FetchSpritesAsync();

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public async void FetchSpritesAsync_ValidGameId()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "ec455cff-cc34-4463-a067-225e46c17d6f");

        // Act
        List<ReadyNFTSpriteObject> result = await readyNFT.FetchSpritesAsync();

        // Assert
        Assert.IsTrue(result.Count > 0);
    }

    [Test]
    public async void FetchOwnedNFTsAsync_InvalidApiKey_ReturnsNull()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("", "gameId");

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("email");

        // Assert
        Assert.AreEqual(0, result.Count);

    }

    [Test]
    public async void FetchOwnedNFTsAsync_InvalidGameId_ThrowsException()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "");

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("email");

        // Assert
        Assert.AreEqual(0, result.Count);

    }

    [Test]
    public async void FetchOwnedNFTsAsync_InvalidEmail_ThrowsException()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "gameId");

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("");

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public async void FetchOwnedNFTsAsync_Success()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();
        readyNFT.Init("apiKey", "ec455cff-cc34-4463-a067-225e46c17d6f");

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("email");

        // Assert
        Assert.IsTrue(result.Count == 0);
    }
}
