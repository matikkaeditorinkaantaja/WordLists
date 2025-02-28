﻿using WordDataAccessLibrary;
using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.Extensions;

namespace WordDataTests.ServiceTests.ExportServiceTests;
public class JsonWordCollectionExportServiceTests
{
	[Fact]
	public void ConvertDataToJson_WithNull_ShouldThrow_ArgumentException()
	{
        // Arrange & Act
        ExportActionResult result = WordCollectionExportService.ConvertDataToJson(null).result;

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void ConvertDataToJson_WithEmptyList_ShouldReturn_SuccessFalse()
    {
        // Arrange & Act
        ExportActionResult result = WordCollectionExportService.ConvertDataToJson(new List<IExportWordCollection>()).result;

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void ConvertDataToJson_ListLengthOne_ParseJson_ShouldNotReturnEmptyString()
    {
        // Arrange
        List<IExportWordCollection> collections = new()
        {
            new DefaultExportWordCollection()
            {
                Description = "this is description",
                Name = "name",
                LanguageHeaders = "fi-en",
                WordPairs = new ExportWordPair[]
                {
                    new("native", "foreign", 1, 3)
                }
            }
        };
        // Act
        string result = WordCollectionExportService.ConvertDataToJson(collections).json;

        // Assert
        Assert.NotEmpty(result);
    }



    [Fact]
    public void ResetLearnStates_ShouldSet_LearnStates_NotSet()
    {
        // Arrange
        List<WordCollection> collections = new()
        {
            new()
            {
                WordPairs =
                {
                    new()
                    {
                        LearnState = WordLearnState.Learned,
                    },
                    new()
                    {
                        LearnState = WordLearnState.MightKnow,
                    },
                    new()
                    {
                        LearnState = WordLearnState.NeverHeard
                    },
                    new()
                    {
                        LearnState = WordLearnState.NotSet
                    }
                }
            },
            new()
            {
                WordPairs =
                {
                    new()
                    {
                        LearnState = WordLearnState.Learned
                    }
                }
            }
        };

        // Act
        collections = collections.ResetLearnStates();

        // Assert
        foreach (var collection in collections)
        {
            foreach (var word in collection.WordPairs)
            {
                Assert.Equal(WordLearnState.NotSet, word.LearnState);
            }
        }
    }
    
    [Fact]
    public void ResetLearnStates_WithNull_ShouldReturn_EmptyList()
    {
        // Arrange & Act
        List<WordCollection> collections = WordCollectionExtensions.ResetLearnStates(null);

        // Assert    
        Assert.Empty(collections);
    }

    [Fact]
    public void ResetLearnStates_WithEmptyList_ShouldReturn_EmptyList()
    {
        // Arrange 
        List<WordCollection> collections = new();

        // Act 
        collections = collections.ResetLearnStates();

        // Assert
        Assert.Empty(collections);
    }
}
