using System.ComponentModel;
using System.Windows.Input;
using WordListsUI.Helpers;

namespace WordListsUI.Components.Entry;

#nullable enable

public partial class PlatformIndependantEntry : Border
{
    public PlatformIndependantEntry()
    {
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set 
        {
            SetValue(TextProperty, value); 
        }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(PlatformIndependantEntry), string.Empty, BindingMode.TwoWay);

    public string PlaceHolder
    {
        get { return (string)GetValue(PlaceHolderProperty); }
        set { SetValue(PlaceHolderProperty, value); }
    }

    public static readonly BindableProperty PlaceHolderProperty =
        BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(PlatformIndependantEntry), string.Empty, BindingMode.TwoWay);

    public int MaxLength
    {
        get { return (int)GetValue(MaxLengthProperty); }
        set { SetValue(MaxLengthProperty, value); }
    }

    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(PlatformIndependantEntry), int.MaxValue);

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(PlatformIndependantEntry));

    public ICommand? TextChangedCommand
    {
        get { return (ICommand?)GetValue(TextChangedCommandProperty); }
        set { SetValue(TextChangedCommandProperty, value); }
    }
    public static readonly BindableProperty TextChangedCommandProperty =
        BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(PlatformIndependantEntry), default(ICommand?));


    public Keyboard Keyboard
    {
        get { return (Keyboard)GetValue(KeyboardProperty); }
        set { SetValue(KeyboardProperty, value); }
    }

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(PlatformIndependantEntry), Keyboard.Default);


    private void ITextInput_Focused(object sender, FocusEventArgs e)
    {
        if (sender is ITextInput input)
        {
            UIInteractionHelper.FocusITextInputText(input, this);
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextChangedCommand?.Execute(null);
    }
}