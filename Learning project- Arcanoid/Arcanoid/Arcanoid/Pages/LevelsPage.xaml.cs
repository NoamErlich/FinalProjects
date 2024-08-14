using DatabaseProject.Models;
using GameEngine.GameServices;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Arcanoid.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelsPage : Page
    {
        public static Level level = new Level();

        public LevelsPage()
        {
            this.InitializeComponent();
        }

        private void imgReturn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/RightArrow (4).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }
        private void imgReturn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgReturn.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/RightArrow (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
        private void imgReturn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(MenuPage));
        }

        private void btn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.BorderBrush = new SolidColorBrush(Colors.Orange);
            btn.BorderThickness = new Thickness(4);
            SoundPlayer.Play("Click.Wav");
        }

        private void btn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void level1_PointerPressed(object sender, RoutedEventArgs e)
        {
            level.LevelNumber = 1;
            level.BarWidth = 300;
            level.Speed = 2;
            level.CountGreenBrick = 8;
            level.CountPinkBrick = 8;
            level.CountYellowBrick = 8;

            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(MenuPage));

        }

        private void level2_PointerPressed(object sender, RoutedEventArgs e)
        {
            level.LevelNumber = 2;
            level.BarWidth = 250;
            level.Speed = 2;
            level.CountGreenBrick = 16;
            level.CountPinkBrick = 8;
            level.CountYellowBrick = 8;

            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(MenuPage));
        }

        private void level3_PointerPressed(object sender, RoutedEventArgs e)
        {
            level.LevelNumber = 3;
            level.BarWidth = 200;
            level.Speed = 2;
            level.CountGreenBrick = 8;
            level.CountPinkBrick = 16;
            level.CountYellowBrick = 8;

            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(MenuPage));
        }

        private void level4_PointerPressed(object sender, RoutedEventArgs e)
        {
            level.LevelNumber = 4;
            level.BarWidth = 150;
            level.Speed = 2;
            level.CountGreenBrick = 0;
            level.CountPinkBrick = 8;
            level.CountYellowBrick = 24;

            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(MenuPage));
        }

    }
}
