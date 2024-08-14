using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Billiard.GameServices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        /// <summary>
        /// Initializing the page.
        /// </summary>
        public AboutPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The method is called when the page is loaded, sets the Height and Width for the main grid.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainGrid.Height = ActualHeight;
            mainGrid.Width = ActualWidth;
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
        /// Makes a sound and navigate to a different page.
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
