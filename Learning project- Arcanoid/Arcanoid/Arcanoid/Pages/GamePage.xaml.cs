using Arcanoid.GameObjects;
using Arcanoid.GameServices;
using DatabaseProject.Models;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.System;
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

namespace Arcanoid.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private bool imgMusicPressed = false;
        private GameManager _gameManager;

        public GamePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _gameManager = new GameManager(scene);
            _gameManager.Start();
            Coins.Text = "" + GameManager.User.Points;
            Manager.GameEvent.OnRemoveHeart += RemoveHeart;
            Manager.GameEvent.OnRemoveBrick += OnRemoveBrick;

        }

        private void OnRemoveBrick()
        {
            int Score= ++GameManager.User.Points;
            Coins.Text = Score.ToString();

            if (scene.BrickCount == 0)
            {
                WonGrid.Visibility = Visibility.Visible;
                _gameManager.Pause();
            }

        }

        private void RemoveHeart(int lifes)
        {
            SoundPlayer.Play("LostLife.wav");

            if (lifes == 2)
                heart1.Visibility = Visibility.Collapsed;

            if(lifes == 1)
                heart2.Visibility = Visibility.Collapsed;

            if (lifes == 0)
            {
                heart3.Visibility = Visibility.Collapsed;
                LostGrid.Visibility = Visibility.Visible;
                _gameManager.Pause();
            }

        }

        private void imgHelp_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgHelp.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Help (4).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgHelp_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgHelp.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Help (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgHelp_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(InfoPage));
        }

        private void imgMusic_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgMusic.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Music (4).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgMusic_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!imgMusicPressed)
                imgMusic.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Music (3).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
        private void imgMusic_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");

            if (!imgMusicPressed)
            {
                MusicPlayer.Stop();
                imgMusicPressed = true;
                imgMusic.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Music (4).png"));
            }

            else
            {
                MusicPlayer.Play("musicForArcanoid.mp3");
                imgMusicPressed = false;
                imgMusic.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Music (3).png"));
            }

        }

        private void imgMusic_Loaded(object sender, RoutedEventArgs e)
        {
            if(MusicPlayer.IsOn == false)
            {
                imgMusicPressed = true;
                imgMusic.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Music (4).png"));
            }
        }

        private void imgPause_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgPause.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Pause (4).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgPause_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgPause.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Pause (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgPause_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (WonGrid.Visibility == Visibility.Collapsed && LostGrid.Visibility == Visibility.Collapsed)
            {
                returnGrid.Visibility = Visibility.Visible;
                _gameManager.Pause();
                SoundPlayer.Play("Click.Wav");
            }
        }

        private void imgpopupExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgpopupExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgpopupExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgpopupExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgpopupExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _gameManager.Resume();
            SoundPlayer.Play("Click.Wav");
            returnGrid.Visibility = Visibility.Collapsed;
        }

        private void imgpopupVerify_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgpopupVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgpopupVerify_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgpopupVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgpopupVerify_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            _gameManager.ResetEvents();
            Frame.Navigate(typeof(MenuPage));
        }

        private void imgPlayAgainVerify_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            LostGrid.Visibility = Visibility.Collapsed;
            WonGrid.Visibility = Visibility.Collapsed;
            heart1.Visibility = Visibility.Visible;
            heart2.Visibility = Visibility.Visible;
            heart3.Visibility = Visibility.Visible;
            _gameManager.ResetEvents();
            Frame.Navigate(typeof(GamePage));
        }

        private void imgLostExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgLostExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgLostExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgLostExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgLostPlayAgainVerify_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgLostPlayAgainVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgLostPlayAgainVerify_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgLostPlayAgainVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgWonExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgWonExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgWonExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgWonExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Cross (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgWonPlayAgainVerify_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgWonPlayAgainVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgWonPlayAgainVerify_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgWonPlayAgainVerify.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Check (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
    }
}
