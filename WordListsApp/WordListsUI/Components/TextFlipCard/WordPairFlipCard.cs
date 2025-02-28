﻿using WordDataAccessLibrary;
using WordListsMauiHelpers;

namespace WordListsUI.Components.TextFlipCard;

/// <summary>
/// 
/// </summary>
public class WordPairFlipCard : AnimatedFlipCard
{
    public WordPairFlipCard() : base()
    {
        WordPair = new("top of WordPairFlipCard", "bottom of WordPairFlipCard");
    }

    public WordPairFlipCard(WordPair pair, bool showNativeFirst = true) : base()
    {
        WordPair = pair;
        ShowNativeFirst = showNativeFirst;
    }

    public WordPair WordPair
    {
        get => (WordPair)GetValue(WordPairProperty);
        set
        {
            SetValue(WordPairProperty, value);
        }
    }
    public bool ShowNativeFirst { get; set; } = true;
    public override string TopSideText
    {
        get => base.TopSideText;
        set => SetValue(TopTextProperty, value);
    }
    public override string BottomSideText
    {
        get => base.BottomSideText;
        set => SetValue(BottomTextProperty, value);
    }


    public static readonly BindableProperty WordPairProperty =
        BindableProperty.Create(nameof(WordPair), typeof(WordPair), typeof(FlipCard), propertyChanged: WordPairProperty_Changed);
    
    
    protected static void WordPairProperty_Changed(BindableObject bindable, object oldValue, object newValue)
    {
        WordPair pair = (WordPair)newValue;
        if (pair is null) return;
        ((WordPairFlipCard)bindable).ShowNewPair(pair);
    }
    protected virtual void ShowNewPair(WordPair pair)
    {
        TopSideText = pair.NativeLanguageWord;
        BottomSideText = pair.ForeignLanguageWord;
        CardColor = GetColor(pair.LearnState);
        if (ShowNativeFirst)
        {
            ShowOrUpdateTop();
            return;
        }
        ShowOrUpdateBottom();
    }
    protected virtual void ShowOrUpdateTop()
    {
        if (ShowingTopSide)
        {
            UpdateText();
            return;
        }
        ShowTopSideNoAnimation();
    }
    protected virtual void ShowOrUpdateBottom()
    {
        if (ShowingTopSide is false)
        {
            UpdateText();
            return;
        }
        ShowBottomSideNoAnimation();
    }
    protected virtual Color GetColor(WordLearnState state)
    {
        return state switch
        {
            WordLearnState.Learned => ColorHelper.GetResourceColor(ResourceColor.LearnedWord),
            WordLearnState.MightKnow => ColorHelper.GetResourceColor(ResourceColor.MightKnowWord),
            WordLearnState.NeverHeard => ColorHelper.GetResourceColor(ResourceColor.NeverHeardWord),
            WordLearnState.NotSet => ColorHelper.GetResourceColor(ResourceColor.DefaultFlipCard),
            _ => ColorHelper.GetResourceColor(ResourceColor.DefaultFlipCard)
        };
    }
}
