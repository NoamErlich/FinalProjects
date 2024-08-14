using Arcanoid.Pages;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Arcanoid
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
        {
            this.InitializeComponent();
        }

        private void imgExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Exit (4).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Exit (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            popupGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Collapsed; //closing other windows so the exit menu will be the only window on the screen.
            SoundPlayer.Play("Click.Wav");
        }

        private void imgShop_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgShop.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Shop (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgShop_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgShop.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Shop (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgPlay_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgPlay.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Play (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgPlay_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgPlay.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Play (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgPlay_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(GamePage));
        }

        private void imgUserInfo_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgUserInfo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Profile (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgUserInfo_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgUserInfo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Profile (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
        private void imgUserInfo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(SingInUpPage));
        }

        private void imgSettings_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgSettings.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Options (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgSettings_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgSettings.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Options (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgSettings_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (SettingsGrid.Visibility == Visibility.Collapsed)
                SettingsGrid.Visibility = Visibility.Visible;

            else
                SettingsGrid.Visibility = Visibility.Collapsed;

            popupGrid.Visibility = Visibility.Collapsed; //closing other windows so the settings window will be the only window on the screen.
            SoundPlayer.Play("Click.Wav");
        }

        private void imgLevels_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgLevels.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/LevelList (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgLevels_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgLevels.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/LevelList (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgLevels_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(LevelsPage));
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
            popupGrid.Visibility = Visibility.Collapsed;
            SoundPlayer.Play("Click.Wav");
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
            App.Current.Exit();
        }

        private void imgInfo_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imgInfo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/info (2).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void imgInfo_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgInfo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/info (1).png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void imgInfo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(InfoPage));
        }


        private void backgroundMusicSw_Toggled(object sender, RoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            if (backgroundMusicSw.IsOn)
                    MusicPlayer.Play("MusicForArcanoid.mp3");

            else
                MusicPlayer.Stop();
        }

        private void backgroundSoundEffectsSw_Toggled(object sender, RoutedEventArgs e)
        {
            if (backgroundSoundEffectsSw.IsOn)
                SoundPlayer.IsOn = true;
            else
                SoundPlayer.IsOn = false;

            SoundPlayer.Play("Click.Wav");
        }
        private void SfxVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SoundPlayer.Volume = SfxVolumeSlider.Value;
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MusicPlayer.Volume = MusicVolumeSlider.Value;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundMusicSw.IsOn = MusicPlayer.IsOn; //כך נזכרים מה היה מצבו של מפסק המוזיקה
            MusicVolumeSlider.Value = MusicPlayer.Volume; //כך נזכרים מה היה רמת העוצמה של המוזיקה
            backgroundSoundEffectsSw.IsOn = SoundPlayer.IsOn; //כך נזכרים מה היה מצבו של מפסק האפקטים
            SfxVolumeSlider.Value = SoundPlayer.Volume; //כך נזכרים מה היה רמת העוצמה של סאונד אפקטס
        }

    }
}
