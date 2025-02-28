﻿
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordListsViewModels;

public partial class WordDataViewModel : ObservableObject, IWordDataViewModel
{
    public WordDataViewModel(
        IWordCollectionService collectionService, 
        IWordPairService pairService)
    {
        CollectionService = collectionService;
        PairService = pairService;
        UpdateData.Execute(null);
    }

    [ObservableProperty]
    int _wordListCount = 0;

    [ObservableProperty]
    int _wordCount = 0;
    
    [ObservableProperty]
    int _learnedWordCount = 0;

    [ObservableProperty]
    int _neverHeardWordCount = 0;

    [ObservableProperty]
    int _mightKnowWordCount = 0;

    [ObservableProperty]
    int _notSetWordCount = 0;

    public IAsyncRelayCommand UpdateData => new AsyncRelayCommand(async () =>
    {
        WordListCount = await CollectionService.CountItems();
        WordCount = await PairService.CountItems();
        LearnedWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.Learned);
        MightKnowWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.MightKnow);
        NeverHeardWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.NeverHeard);
        NotSetWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.NotSet);
    });

    IWordCollectionService CollectionService { get; }
    public IWordPairService PairService { get; }
}
