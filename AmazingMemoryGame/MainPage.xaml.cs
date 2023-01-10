using AmazingMemoryGame.Models;

namespace AmazingMemoryGame;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        CreateButtons();

    }

    private void CreateButtons()
    {
        int nOfCards = 16;
        for (int i = 0; i < nOfCards; i++)
        {
            Button button = new Button();
            button.Clicked += OnButtonClicked;
            button.BackgroundColor = Colors.Gray;
            button.BindingContext = new ButtonViewModel { Id = i*2%3, Text = "Button " + i };
            
            Grid.SetRow(button, (int)(i /Math.Sqrt(nOfCards)));
            Grid.SetColumn(button, (int)(i % Math.Sqrt(nOfCards)));
            mainGrid.Children.Add(button);
        }
    }


    private void OnButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        var buttonViewModel = (ButtonViewModel)button.BindingContext;

        // check if the button is already selected
        if (buttonViewModel.IsSelected)
        {
            // deselect the button
            buttonViewModel.IsSelected = false;
            button.BackgroundColor = Colors.Gray;
        }
        else
        {
            // select the button
            buttonViewModel.IsSelected = true;
            button.BackgroundColor = Colors.Red;
            CheckForMatch();
        }
    }

    private async void CheckForMatch()
    {
        List<Button> selectedButtons = new List<Button>();
        //get all the selected button
        foreach (var child in mainGrid.Children)
        {
            if (child is Button b && b.BackgroundColor == Colors.Red)
            {
                selectedButtons.Add(b);
            }
        }
        // check if there are exactly two selected buttons
        if (selectedButtons.Count == 2)
        {
            var button1 = (ButtonViewModel)selectedButtons[0].BindingContext;
            var button2 = (ButtonViewModel)selectedButtons[1].BindingContext;

            if (button1.Id == button2.Id)
            {
                button1.IsMatched = true;
                button2.IsMatched = true;
                selectedButtons[0].BackgroundColor= Colors.Violet;
                selectedButtons[1].BackgroundColor= Colors.Violet;
                selectedButtons[0].Opacity = 20;
                selectedButtons[1].Opacity = 20;
                selectedButtons[0].IsEnabled = false;
                selectedButtons[1].IsEnabled = false;
                // you can add animation here
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                selectedButtons[0].BackgroundColor = Colors.Gray;
                selectedButtons[1].BackgroundColor = Colors.Gray;
                button1.IsSelected = false;
                button2.IsSelected = false;
            }
        }

    }



}

