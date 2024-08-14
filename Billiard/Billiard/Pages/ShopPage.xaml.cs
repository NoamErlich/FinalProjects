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
using Windows.UI.Popups;
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
    public sealed partial class ShopPage : Page
    {
        /// <summary>
        /// Initializing the page.
        /// </summary>
        public ShopPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The method is called when the page is loaded, sets the Height and Width 
        /// for the main grid, and updates the shop items list.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainGrid.Height = ActualHeight;
            mainGrid.Width = ActualWidth;
            UpdateShopItems();
        }

        /// <summary>
        /// Updates the list of available items in the shop based on the user's inventory. 
        /// For each table owned by the user, this method creates a visual representation 
        /// consisting of an image and a text block displaying the table's name and price. 
        /// The method iterates through each table in the user's inventory, creates a stack
        /// panel to contain the visual elements, sets the orientation, width, and margin 
        /// properties of the stack panel, creates an image element for the table, sets its 
        /// dimensions and source to the corresponding table image, creates a text block to 
        /// display the table's name and price, sets its font, size, color, and margin properties,
        /// adds the image and text block to the stack panel, and finally adds the stack panel to the
        /// shop list for display.
        /// </summary>
        private void UpdateShopItems()
        {
            StackPanel stackPanel = null;
            Image imgTable = null;
            TextBlock textBlock = null;
            foreach (string name in GameManager.User.userTables)
            {
                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Width = ShopList.ActualWidth;
                stackPanel.Margin = new Thickness(11);

                imgTable = new Image()
                {
                    Width = 250,
                    Height = 250,
                    Source = new BitmapImage(new Uri($"ms-appx:///Assets/Tables/{name}.png")),

                };
                textBlock = new TextBlock()
                {
                    Text = name + " - $" + 300 * GameManager.User.userTables.IndexOf(name),
                    FontFamily = new FontFamily("Burbank Big Cd Bk"),
                    FontSize = 80,
                    Foreground = new SolidColorBrush(Colors.FloralWhite),
                    Margin = new Thickness(35)
                };

                stackPanel.Children.Add(imgTable);
                stackPanel.Children.Add(textBlock);
                ShopList.Items.Add(stackPanel);
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

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgBuy_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgBuy.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Buy_Equip.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgBuy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgBuy.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Buy_Equip.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes a sound and equips or buys (if player doesn't own yet the item and has enough money) the selected table.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void imgBuy_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.wav");

            int index = ShopList.SelectedIndex;
            string productName = GameManager.User.UsingProductName;

            switch (index)
            {
                case 0:
                    productName = "GreenTable";
                    break;

                case 1:
                    productName = "RedTable";
                    break;

                case 2:
                    productName = "BlueTable";
                    break;

                case 3:
                    productName = "OrangeTable";
                    break;

                case 4:
                    productName = "PinkTable";
                    break;
            }

            //if player owns the table already
            if (Server.IsHavingTheProduct(GameManager.User, productName))
            {
                GameManager.User.UsingProductName = productName;
                GameManager.UpdateData();//saves data in database
                await new MessageDialog("", $"{productName} Equipped").ShowAsync();
            }

            //if he doesn't own the table, check if he can buy it.
            else
            {
                if (GameManager.User.Money >= index * 300) //if user can buy the iem
                {
                    GameManager.User.Money -= index * 300; //decreasing user money after his purchase
                    GameManager.User.UsingProductName = productName; //sets the current product to the new one that user bought
                    Server.AddUserProduct(GameManager.User.UserId, productName); //adds player product in database
                    GameManager.UpdateData(); //saves data in database
                    await new MessageDialog("", "Purchase success").ShowAsync();
                }
                else //if user cant buy the item
                {
                    await new MessageDialog("", $"Unfortunately ,you don't have enough money to buy {productName}").ShowAsync();
                }

            }
        }
    }
}
