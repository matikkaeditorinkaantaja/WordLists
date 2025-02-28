﻿using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IWriteWordViewModel
{
    /// <summary>
    /// Start new collection with random word pair order. 
    /// If pair count is provided and it is valid, only that amount of word pairs will be chosen randomly.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="pairCount"></param>
    void StartNew(WordCollection collection);
    List<WordPairQuestion> Questions { get; }
    IAsyncRelayCommand ValidateAll { get; }
    WordCollectionOwner Info { get; }
    string SessionId { get; }
    bool SaveProgression { get; set; }

    event TestValidatedEventHandler TestValidated;
}
