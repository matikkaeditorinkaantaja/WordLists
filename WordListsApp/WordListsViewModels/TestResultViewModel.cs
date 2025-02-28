﻿using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

public partial class TestResultViewModel : ObservableObject, ITestResultViewModel
{
    public TestResultViewModel(IWordPairService wordPairService)
    {
        WordPairService = wordPairService;
    }

    List<WordPairQuestion> _answeredQuestions = new();

    public List<WordPairQuestion> AnsweredQuestions 
    { 
        get => _answeredQuestions;
        set 
        {
            _answeredQuestions = value;
            CalculateAndSetScore();
            OnPropertyChanged(nameof(AnsweredQuestions));
        }
    }

    private void CalculateAndSetScore()
    {
        double questionCount = AnsweredQuestions.Count;
        if (questionCount is 0) questionCount = 1;  // handle divided by zero exception

        // percentage of right answers
        int rightAnswers = AnsweredQuestions.Where(x => x.MatchResult?.IsFullMatch is true).Count();
        double preciseScore = rightAnswers / questionCount * 100d;
        Score = Convert.ToInt32(preciseScore);

        // average percentage calculated from char match percentages
        int charMatchPercentageSum = AnsweredQuestions.Sum(x => x.MatchResult?.CharMatchPercentage ?? 0);
        double precise = charMatchPercentageSum / questionCount;
        CharMatchPercentage = Math.Round(precise, 5);
    }

    [ObservableProperty]
    int _score = 0;

    [ObservableProperty]
    double _charMatchPercentage = 0d;

    [ObservableProperty]
    string _sessionId = "#0000000";

    [ObservableProperty]
    //[AlsoNotifyChangeFor(nameof(ProgressionNotSaved))]
    [NotifyPropertyChangedFor(nameof(ProgressionNotSaved))]
    bool _progressionSaved = false;
    
    public bool ProgressionNotSaved => !ProgressionSaved;

    public IAsyncRelayCommand SaveProgression => new AsyncRelayCommand(async () =>
    {
        await AnsweredQuestions.SaveLearnStates(WordPairService);
        ProgressionSaved = true;
    });

    public IRelayCommand ExitResultsCommand => new RelayCommand(() =>
    {
        ExitResults?.Invoke(this, EventArgs.Empty);
    });

    IWordPairService WordPairService { get; }

    public event ExitResultsEventHandler? ExitResults;
}
