using System;
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
        ReadyNFTFileHelpers RNFTHelpers = new ReadyNFTFileHelpers();
        readyNFT.Init("apiKey", "8f85c1eb-d3e9-4cd7-b8ed-640abab2770c");

        // Act
        List<ReadyNFTSpriteObject> result = await readyNFT.FetchSpritesAsync();

        // Assert
        Assert.IsTrue(result.Count == 0); // as the api key is invalid

        // save the sprites onto the device
        Progress<ReadyNFTDownloadReport> reporter = new Progress<ReadyNFTDownloadReport>(report =>
        {
            Debug.Log("Files downloaded: " + report.current + "/" + report.total + " (" + report.percent + "%)");
        });

        await RNFTHelpers.DownloadSpriteImagesAsync(result, reporter);
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
        readyNFT.Init("apiKey", "8f85c1eb-d3e9-4cd7-b8ed-640abab2770c");

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("email");

        // Assert
        Assert.IsTrue(result.Count == 0);
    }

    // test to delete all the files
    [Test]
    public void DeleteAllImages_Success()
    {
        // Arrange
        ReadyNFTFileHelpers fileHelper = new ReadyNFTFileHelpers();

        // Act
        fileHelper.DeleteAllImages();
        int files = fileHelper.GetNumberOfImages();

        // Assert
        Assert.IsTrue(files == 0);
    }
}
