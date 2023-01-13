using AmazingMemoryGame.Models;
using NCalc;
using System.Linq;

namespace AmazingMemoryGame;

public partial class GamePage : ContentPage
{
    List<Button> buttons = new List<Button>();
    List<Image> buttons_images = new List<Image>();
    List<Image> skins = new List<Image>();
    List<string> IDs_list = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "A", "B", "C", "D", "E", "F", "G", "H" }.OrderBy(a => Guid.NewGuid()).ToList();
    List<string> card_backs = new List<string> { "back_card_clubs.png", "back_card_clubs.png", "back_card_clubs.png", "back_card_clubs.png", "back_card_diamond.png", "back_card_diamond.png", "back_card_diamond.png", "back_card_diamond.png", "back_card_heart.png", "back_card_heart.png", "back_card_heart.png", "back_card_heart.png", "back_card_spades.png", "back_card_spades.png", "back_card_spades.png", "back_card_spades.png" }.OrderBy(a => Guid.NewGuid()).ToList();
    

     

    public GamePage()
    {
        InitializeComponent();
        Create();
    }
    

    private async void Create()
    {
        int nOfCards = 16;
        for (int i = 0; i < nOfCards; i++)
        {
            Button button = new Button();
            button.Clicked += OnButtonClicked;
            button.BackgroundColor = Colors.Transparent;
            button.BindingContext = new CardModel { Id = IDs_list[i], Text = $"Button {i}", Index=i};

            Image backImage = new Image();
            backImage.Source = card_backs[i];            
            backImage.InputTransparent = true;

            Image skin = new Image();
            skin.Source = $"Images/{IDs_list[i]}.svg";
            skin.IsVisible = true;
            await skin.ScaleTo(0, 250);
            //skin.IsVisible = false;


            Grid.SetRow(button, (int)(i /Math.Sqrt(nOfCards)));
            Grid.SetColumn(button, (int)(i % Math.Sqrt(nOfCards)));
            
            Grid.SetRow(backImage, (int)(i /Math.Sqrt(nOfCards)));
            Grid.SetColumn(backImage, (int)(i % Math.Sqrt(nOfCards)));

            Grid.SetRow(skin, (int)(i / Math.Sqrt(nOfCards)));
            Grid.SetColumn(skin, (int)(i % Math.Sqrt(nOfCards)));

            mainGrid.Children.Add(button);
            mainGrid.Children.Add(backImage);

            buttons.Add(button);
            buttons_images.Add(backImage);
            skins.Add(skin);
        }
    }


    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        var buttonViewModel = (CardModel)button.BindingContext;
        
        Image backImage = (Image)buttons_images[buttonViewModel.Index];
        // check if the button is already selected
        if (buttonViewModel.IsSelected)
        {
            // deselect the button
            buttonViewModel.IsSelected = false;
            button.BackgroundColor = Colors.Transparent;
            await backImage.ScaleTo(1, 250);
            //await backImage.ScaleTo(1, 250, Easing.CubicOut);
            backImage.IsVisible = true;
        }
        else
        {
            // select the button
            buttonViewModel.IsSelected = true;
            button.BackgroundColor = Colors.CadetBlue;
            backImage.IsVisible = true;
            await backImage.ScaleTo(0, 250);
            //await backImage.ScaleTo(0, 250, Easing.CubicIn);
            CheckForMatch();
        }
    }

    private async void CheckForMatch()
    {
        List<Button> selectedButtons = new List<Button>();
        
        //get all the selected button
        foreach (var child in mainGrid.Children)
        {
            if (child is Button b && b.BackgroundColor == Colors.CadetBlue)
            {
                selectedButtons.Add(b);
            }
        }
        // check if there are exactly two selected buttons
        if (selectedButtons.Count == 2)
        {
            foreach (var child in mainGrid.Children)
            {
                if (child is Button b)
                    b.IsEnabled = false;
            }

            var button1 = (CardModel)selectedButtons[0].BindingContext;
            var button2 = (CardModel)selectedButtons[1].BindingContext;

            Image backImage1 = (Image)buttons_images[button1.Index];
            Image backImage2 = (Image)buttons_images[button2.Index];

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

                CheckIfFinished();
                //ReEnableButtons();
                foreach (var child in mainGrid.Children)
                {
                    if (child is Button b)
                    {
                        var b_cardModel = (CardModel)b.BindingContext;
                        if (b_cardModel.IsMatched)
                            continue;
                        b.IsEnabled = true;
                    }
                }
            }
            else
            {

                await Task.Delay(TimeSpan.FromMilliseconds(500));
                selectedButtons[0].BackgroundColor = Colors.Transparent;
                selectedButtons[1].BackgroundColor = Colors.Transparent;
                button1.IsSelected = false;
                button2.IsSelected = false;

                mainGrid.IsEnabled = true;
                
                //ReEnableButtons();
                foreach (var child in mainGrid.Children)
                {
                    if (child is Button b)
                    {
                        var b_cardModel = (CardModel)b.BindingContext;
                        if (b_cardModel.IsMatched)
                            continue;
                        b.IsEnabled = true;
                    }
                }
                //await backImage1.ScaleTo(1, 250, Easing.CubicOut);
                //await backImage2.ScaleTo(1, 250, Easing.CubicOut);
                await backImage1.ScaleTo(1, 250);
                await backImage2.ScaleTo(1, 250);
            }
        }
    }

    private void CheckIfFinished()
    {
        int matchedCards = 0;
        foreach (var child in mainGrid.Children)
        {
            if (child is Button b)
            {
                var buttonViewModel = (CardModel)b.BindingContext;
                if (buttonViewModel.IsMatched)
                {
                    matchedCards++;
                }
            }
        }
        if (matchedCards == IDs_list.Count)
        {
            // all cards have been matched, game is finished
            GameEnded();
        }
    }

    //private void ReEnableButtons()
    //{
    //    foreach (var child in mainGrid.Children)
    //    {
    //        if (child is Button b)
    //        {
    //            var b_cardModel = (CardModel)b.BindingContext;
    //            if (b_cardModel.IsMatched)
    //                continue;
    //            b.IsEnabled = true;
    //        }
    //    }
    //}

    private async void GameEnded()
    {
        await DisplayAlert("Congratulations", "You have won the game!", "OK");

        await Navigation.PopAsync();
    }
}

