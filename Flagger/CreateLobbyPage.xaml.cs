using Flagger.Client;
using Flagger.Models;
namespace Flagger;

[QueryProperty(nameof(flaggerClient), "Client")]
public partial class CreateLobbyPage : ContentPage
{
    public FlaggerClient flaggerClient { set { if (value != null) ((CreateLobbyModel)BindingContext).FlaggerClient = value; } }

    public CreateLobbyPage()
	{
		InitializeComponent();
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            bool isWholeNumber = int.TryParse(e.NewTextValue, out int value) && value > 0;
            if (!isWholeNumber)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
        else
        {
            ((Entry)sender).Text = null;
        }
    }
}