namespace AmazingMemoryGame;

public partial class LandingPage : ContentPage
{
	public LandingPage()
	{
		InitializeComponent();
	}

    private async void StartGameButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }
}