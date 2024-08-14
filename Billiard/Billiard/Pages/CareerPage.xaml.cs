using Billiard.GameServices;
using DatabaseProject;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Billiard.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CareerPage : Page
    {
        /// <summary>
        /// Initializing the page.
        /// </summary>
        public CareerPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// The method is called when the page is loaded, sets the Height and Width for the
        /// main grid, and updates the Career list.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainGrid.Height = ActualHeight;
            mainGrid.Width = ActualWidth;
            UpdateCareer();
        }

        /// <summary>
        /// Updates the Career list with the players' information and their money. 
        /// Player information includes their position, username, and money. 
        /// The list is sorted based on the players' money in descending order.
        /// </summary>
        private void UpdateCareer()
        {
            // Initialize player position
            int playerTop = 1;

            // Initialize stack panel and text block
            StackPanel stackPanel = null;
            TextBlock textBlock = null;

            // Get dictionary of all players and their money
            Dictionary<string, int> playersMoneyDict = Server.ReturnAllPlayersMoney();

            // Sort players by their money in descending order
            var sortedPlayers = playersMoneyDict.OrderByDescending(x => x.Value);

            // Iterate through sorted players
            foreach (var player in sortedPlayers)
            {
                // Create a new stack panel
                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Width = CareerList.ActualWidth;
                stackPanel.Margin = new Thickness(11);

                // Set foreground color based on whether the player is the current user
                if (GameManager.User.UserName == player.Key)
                    Foreground = new SolidColorBrush(Colors.Gold);
                else
                    Foreground = new SolidColorBrush(Colors.FloralWhite);

                // Create a new text block with player information
                textBlock = new TextBlock()
                {
                    Text = "#" + playerTop++ + " " + player.Key + "  |  Money: $" + player.Value,
                    FontFamily = new FontFamily("Burbank Big Cd Bk"),
                    Foreground = Foreground,
                    FontSize = 80,
                    Margin = new Thickness(35)
                };

                // Add text block to stack panel
                stackPanel.Children.Add(textBlock);

                // Add stack panel to Career list
                CareerList.Items.Add(stackPanel);
            }
        }


        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Return.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Return.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes a sound and navigates to a MenuPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgReturn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.wav");
            Frame.Navigate(typeof(MenuPage));
        }
    }
}
