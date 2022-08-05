using WordListsMauiHelpers.Factories;
using WordListsUI.Helpers;
using WordDataAccessLibrary.DataBaseActions;
using WordListsViewModels.Interfaces;

namespace WordListsUI.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IAbstractFactory<IListGeneratorViewModel> modelFactory)
	{
		ModelFactory = modelFactory;
		BindingContext = modelFactory.Create();
		InitializeComponent();
        Model.CollectionAddedEvent += Model_CollectionAddedEvent;
    }

	private async void Model_CollectionAddedEvent(object sender, DataBaseActionArgs e)
	{
		BindingContext = ModelFactory.Create();
		await DisplayAlert("Onnistui!", $"Sanasto lis�tty onnistuneesti s�ilytykseen id:ll� {e.RefId}", "OK");
		Model.CollectionAddedEvent -= Model_CollectionAddedEvent;
		Model.CollectionAddedEvent += Model_CollectionAddedEvent;
    }


    IAbstractFactory<IListGeneratorViewModel> ModelFactory { get; }

    public IListGeneratorViewModel Model => (IListGeneratorViewModel)BindingContext;
	
	private void ITextInput_Focused(object sender, FocusEventArgs e)
	{
		if (sender is ITextInput input)
		{
			// this does not work with editor
            UIInteractionHelper.FocusITextInputText(input, this);
		}
	}

	
	private void StartAgainBtn_Click(object sender, EventArgs e)
	{
		BindingContext = ModelFactory.Create();
	}


}