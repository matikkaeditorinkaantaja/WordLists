﻿using WordListsViewModels.Extensions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordListsViewModels.Helpers;
public class WordCollectionInfoService : IWordCollectionInfoService
{
    public WordCollectionInfoService(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    IWordCollectionService CollectionService { get; }

    public async Task<List<WordCollectionInfo>> GetAll()
    {
        List<WordCollection> collections = await CollectionService.GetWordCollections();

        return collections.Select(x => new WordCollectionInfo(x.Owner, x.WordPairs.Count))
                          .ToList()
                          .SortByName();
    }
}
