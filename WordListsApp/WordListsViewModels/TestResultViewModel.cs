﻿using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class TestResultViewModel : ITestResultViewModel
{
    List<WordPairQuestion> _answeredQuestions = Enumerable.Empty<WordPairQuestion>().ToList();

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
        CharMatchPercentage = Math.Round(precise, 4);
    }

    [ObservableProperty]
    int score = 0;

    [ObservableProperty]
    double charMatchPercentage = 0d;

    [ObservableProperty]
    string sessionId = "#0000000";

    public IRelayCommand ExitResultsCommand => new RelayCommand(() =>
    {
        ExitResults?.Invoke(this, EventArgs.Empty);
    });

    public event ExitResultsEventHandler? ExitResults;
}
