using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordListsUI.Components.TextFlipCard;
#if WINDOWS
using Microsoft.UI.Xaml;
using Windows.System;
#endif
namespace WordListsUI.WordTrainingPages.FlipCardTrainingPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
[QueryProperty(nameof(StartWordCollection), nameof(StartWordCollection))]
public partial class FlipCardTrainingPage : ContentPage
{
    public FlipCardTrainingPage(IWordTrainingViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
        _animator = new(flipper);
        Model.CollectionUpdated += Model_CollectionUpdatedEvent;
    }

    public WordCollection StartCollection
    {
        set
        {
            if (value is not null) Model.StartNew(value);
        }
    }

    private async void Model_CollectionUpdatedEvent(object sender, DataBaseActionArgs e)
    {
        await DisplayAlert("P�ivitetty!", $"Sanasto p�ivitettiin s�ilytykseen id:ll� {e.RefIdString}", "OK");
    }

    public int StartWordCollection { set => StartNewCollectionById(value); }

    public IWordTrainingViewModel Model => (IWordTrainingViewModel)BindingContext;

    public bool ShowNativeWordByDefault { get; set; } = true;


    readonly SlideAnimation _animator;

    private void ShowDefaultSideWithoutAnimation()
    {
        if (ShowNativeWordByDefault)
        {
            flipper.ShowTopSideNoAnimation();
            return;
        }
        flipper.ShowBottomSideNoAnimation();
    }

    private async void Button_LastCard(object sender, EventArgs e)
    {
        await _animator.ToMaxRight();
        Model.Previous();
        ShowDefaultSideWithoutAnimation();
        await _animator.FromMaxLeftToMiddle();
    }

    private async void Button_NextCard(object sender, EventArgs e)
    {
        await _animator.ToMaxLeft();
        Model.Next();
        ShowDefaultSideWithoutAnimation();
        await _animator.FromMaxRightToMiddle();
    }

    private void FlipperGrid_SizeChanged(object sender, EventArgs e)
    {
#if WINDOWS
        if (sender is Grid grid)
        {
            WordTrainingPages.FlipCardTrainingPage.Helpers.FlipperResizer.Resize(grid, Width);
        }
#endif
    }

    /// <summary>
    /// This method is called when StartWordCollection is initialized
    /// </summary>
    /// <param name="id"></param>
    private async void StartNewCollectionById(int id)
    {
        await Model.StartNewAsync(id);
    }

#if WINDOWS
    // This does not work currently, because page cannot be focused (needs entry)
    protected override void OnHandlerChanged()
    {
        if (Handler?.PlatformView is UIElement element)
        {
            element.ProcessKeyboardAccelerators += async (sender, args) =>
            {
                switch (args.Modifiers, args.Key)
                {
                    case (VirtualKeyModifiers.Control, VirtualKey.S):
                        await Model.SaveProgression.ExecuteAsync(null);
                        break;
                }
            };
            element.KeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case (VirtualKey.Right):
                        Model.Next();
                        break;
                    case (VirtualKey.Left):
                        Model.Previous();
                        break;
                    case (VirtualKey.Number1):
                        Model.WordLearnedCommand.Execute(null);
                        break;
                    case (VirtualKey.Number2):
                        Model.MightKnowWordCommand.Execute(null);
                        break;
                    case (VirtualKey.Number3):
                        Model.WordNeverHeardCommand.Execute(null);
                        break;
                    case (VirtualKey.Back):
                        Model.WordStateNotSetCommand.Execute(null);
                        break;
                }
            };

        }

    }
#endif
}