using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordTrainingPages.WritingTestPage;

namespace WordListsUI.WordTrainingPages.StartTrainingPage;

public partial class StartTrainingPage : ContentPage
{
    public StartTrainingPage(IStartTrainingViewModel model, IWordCollectionService collectionService)
    {
        BindingContext = model;
        InitializeComponent();
        CollectionService = collectionService;
        Model.WriteTrainingRequestedEvent += Model_WriteTrainingRequestedEvent;
        Model.CardsTrainingRequestedEvent += Model_CardsTrainingRequestedEvent;
        Model.CollectionDoesNotExistEvent += Model_CollectionDoesNotExistEvent;
    }

    private async void Model_CollectionDoesNotExistEvent(object sender, int id, string message)
    {
        await DisplayAlert("Sanastoa ei l�ydy :(", 
            """
            Valitsemaasi sanastoa ei en�� l�ydy, joten se on todenn�k�isesti ehditty poistaa.
            N�kym� on nyt p�ivitetty, joten kannattaa yritt�� uudelleen.
            """, 
            "OK");
    }

    private async void Model_CardsTrainingRequestedEvent(object sender, WordListsViewModels.Events.StartTrainingEventArgs e)
    {
        if (e.WordCollection is null)
        {
            Debug.WriteLine($"{nameof(StartTrainingPage)} Cannot navigate to training page, because given parameter {nameof(e.WordCollection)} is null");
        }

        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.Training)}/{nameof(FlipCardTrainingPage.FlipCardTrainingPage)}", new Dictionary<string, object>()
        {
            ["StartCollection"] = e.WordCollection
        });

    }

    private async void Model_WriteTrainingRequestedEvent(object sender, WordListsViewModels.Events.StartTrainingEventArgs e)
    {
        if (e.WordCollection is null)
        {
            Debug.WriteLine($"{nameof(StartTrainingPage)} Cannot navigate to training page, because given parameter {nameof(e.WordCollection)} is null");
        }

        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.Training)}/{nameof(WritingTestConfigurationPage)}", new Dictionary<string, object>()
        {
            ["StartCollection"] = e.WordCollection
        });
    }

    public IStartTrainingViewModel Model => (IStartTrainingViewModel)BindingContext;

    public IWordCollectionService CollectionService { get; }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await Model.ResetCollections();
    }

    private void CollectionView_Loaded(object sender, EventArgs e)
    {
        if (sender is CollectionView collectionView)
        {
#if WINDOWS
            collectionView.ItemTemplate = (DataTemplate)collectionView.Resources["windowsTemplate"];
#else
            collectionView.ItemTemplate = (DataTemplate)collectionView.Resources["defaultTemplate"];
#endif
        }
    }

 
    private void OwnerTemplate_Loaded(object sender, EventArgs e)
    {
        if (sender is Border border && border.BindingContext is WordCollectionOwner { Name: "^^_$Placeholder$_^^" })
        {
            border.IsVisible = false;
        }
    }
}