using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class ReadyNFTTests
{

    [Test]
    public async void FetchOwnedNFTsAsync_Success()
    {
        // Arrange
        ReadyNFT readyNFT = new ReadyNFT();

        // Act
        List<ReadyNFTOwnedNFTObject> result = await readyNFT.FetchOwnedNFTsAsync("email");

        // Assert
        Assert.IsTrue(result.Count == 0);
    }

    [Test]
    public void DeleteAllImages_Success()
    {
        // Arrange
        RNFTSpriteMetadataHelpers fileHelper = new RNFTSpriteMetadataHelpers();

        // Act
        fileHelper.DeleteAllImages();
        int files = fileHelper.GetNumberOfImages();

        // Assert
        Assert.IsTrue(files == 0);
    }
}
