using Billiard.Pages;
using DatabaseProject;
using GameEngine.GameServices;
using System;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Billiard.GameServices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        /// <summary>
        /// Initializing the page.
        /// </summary>
        public MenuPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event handler for when page is loaded.
        /// Makes sound and sets defualt settings and images.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundMusicSw.IsOn = MusicPlayer.IsOn; //כך נזכרים מה היה מצבו של מפסק המוזיקה
            MusicVolumeSlider.Value = MusicPlayer.Volume; //כך נזכרים מה היה רמת העוצמה של המוזיקה
            backgroundSoundEffectsSw.IsOn = SoundPlayer.IsOn; //כך נזכרים מה היה מצבו של מפסק האפקטים
            SfxVolumeSlider.Value = SoundPlayer.Volume; //כך נזכרים מה היה רמת העוצמה של סאונד אפקטס
            mainGrid.Height = ActualHeight;
            mainGrid.Width = ActualWidth;
            MoneyBar.Text = "$" + GameManager.User.Money;
            UpdateAvatarImages();
            SetUserAvatar();
        }

        /// <summary>
        /// Sets the avatarGrid with all the possible images.
        /// </summary>
        private void UpdateAvatarImages()
        {
            StackPanel stackPanel = null;
            Image imgAvatar = null;
            string[] imageNames = { "DefaultAvatar", "DefaultAvatarGreen", "DefaultAvatarRed", "DefaultAvatarYellow", "DefaultAvatarPink" };
            foreach (string name in imageNames)
            {
                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Width = avatarList.ActualWidth;
                stackPanel.Margin = new Thickness(11);

                imgAvatar = new Image()
                {
                    Width = 1050,
                    Height = 1050,
                    Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{name}.png"))
                };

                stackPanel.Children.Add(imgAvatar);
                avatarList.Items.Add(stackPanel);
                EditAvatarGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Function set the user welcome quote, colors and avatar for the user in the main menu.
        /// </summary>
        private void SetUserAvatar()
        {
            imgUser.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{GameManager.User.UserAvatarPic}.png"));
            Random rnd = new Random();
            int CurrentHour = DateTime.Now.Hour;
            string localTimeQuote = "";

            if (CurrentHour >= 5 && CurrentHour < 12)
                localTimeQuote = "Good morning,";
            else if (CurrentHour >= 12 && CurrentHour < 18)
                localTimeQuote = "Good afternoon,";
            else if (CurrentHour >= 18 && CurrentHour < 22)
                localTimeQuote = "Good evening,";
            else
                localTimeQuote = "Good night,";

            string[] welcomeQuotes = { "Welcome back,", "Good to see you,", localTimeQuote }; //להוסיף הודעה לפי שעה, פשוט שאחד מהסטרינגים יהיה משתנה ששווה לתוכן של פעולה שמחזירה את המשפט הנכון לפי השעה המתאימה
            int randomQuote = rnd.Next(0, 3);

            NameUser.Text = welcomeQuotes[randomQuote] + " " + GameManager.User.UserName;
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgPlay_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgPlay.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Play.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgPlay_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgPlay.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Play.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and navigates to GamePage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgPlay_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(GamePage));
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgAbout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgAbout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_About.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgAbout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgAbout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/About.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and navigates to AboutPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgAbout_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(AboutPage));
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCareer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgCareer.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Career.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCareer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgCareer.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Career.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and navigates to CarrierPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCareer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(CareerPage));
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgShop_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgShop.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Shop.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgShop_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgShop.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Shop.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and navigates to ShopPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgShop_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            Frame.Navigate(typeof(ShopPage));
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgSettings_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgSettings.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Settings.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgSettings_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgSettings.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Settings.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound, opens SettingsGrid and collapses every other grid.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgSettings_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");

            exitGrid.Visibility = Visibility.Collapsed;
            EditAvatarGrid.Visibility = Visibility.Collapsed;
            logOutGrid.Visibility = Visibility.Collapsed;


            if (SettingsGrid.Visibility == Visibility.Collapsed)
                SettingsGrid.Visibility = Visibility.Visible;
            else
                SettingsGrid.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Exit.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Exit.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound, opens exitGrid and collapses every other grid.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
            EditAvatarGrid.Visibility= Visibility.Collapsed;
            logOutGrid.Visibility = Visibility.Collapsed;


            if (exitGrid.Visibility != Visibility.Visible)
            {
                SoundPlayer.Play("Click.Wav");
                exitGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgVerifyExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Yes.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgVerifyExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Yes.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and update user data, and exits the game.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            GameManager.UpdateData(); //updates user data before exiting the game.
            App.Current.Exit();
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelExit_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgCancelExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_No.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelExit_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgCancelExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/No.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and collapse exitGrid
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            exitGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event handler for when the slider is toggled.
        /// Makes sound and turn on\off the music.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void backgroundMusicSw_Toggled(object sender, RoutedEventArgs e)
        {
            string[] musicArr = { "A Kiss For Amanda.wav", "Bluesy Vibes.wav", "Book Bag.wav", "Cocktail Hour.wav", "Ersatz Bossa.wav", "I Have a Reservation.wav", "Peacefully.wav", "Soul and Mind.wav" };

            SoundPlayer.Play("Click.Wav");
            if (backgroundMusicSw.IsOn)
                MusicPlayer.PlayRandom(musicArr);

            else
                MusicPlayer.Stop();
        }

        /// <summary>
        /// Event handler for when the slider is toggled.
        /// Makes sound and turn on\off the sound effects.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void backgroundSoundEffectsSw_Toggled(object sender, RoutedEventArgs e)
        {
            if (backgroundSoundEffectsSw.IsOn)
                SoundPlayer.IsOn = true;
            else
                SoundPlayer.IsOn = false;

            SoundPlayer.Play("Click.Wav");

        }

        /// <summary>
        /// Event handler for when the slider is moved.
        /// Changes the volume value for sound effects.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void SfxVolumeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SoundPlayer.Volume = SfxVolumeSlider.Value;
        }

        /// <summary>
        /// Event handler for when the slider is moved.
        /// Changes the volume value for music.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MusicVolumeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            MusicPlayer.Volume = MusicVolumeSlider.Value;
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgUser_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            imgUser.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{GameManager.User.UserAvatarEditPic}.png"));
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgUser_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            imgUser.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{GameManager.User.UserAvatarPic}.png"));
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and opens EditAvatarGrid and closes every other grid.
        /// Also changes player profile picture if player presses on a new photo.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgUser_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            exitGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            logOutGrid.Visibility = Visibility.Collapsed;

            if (EditAvatarGrid.Visibility == Visibility.Collapsed)
                EditAvatarGrid.Visibility = Visibility.Visible;
            else
            {
                int index = avatarList.SelectedIndex;
                string avatarPic = GameManager.User.UserAvatarPic;
                string editAvatarpic = GameManager.User.UserAvatarEditPic;

                switch (index)
                {
                    case 0:
                        avatarPic = "DefaultAvatar";
                        editAvatarpic = "DefaultAvatarEdit";
                        break;

                    case 1:
                        avatarPic = "DefaultAvatarGreen";
                        editAvatarpic = "DefaultAvatarGreenEdit";
                        break;

                    case 2:
                        avatarPic = "DefaultAvatarRed";
                        editAvatarpic = "DefaultAvatarRedEdit";
                        break;

                    case 3:
                        avatarPic = "DefaultAvatarYellow";
                        editAvatarpic = "DefaultAvatarYellowEdit";
                        break;

                    case 4:
                        avatarPic = "DefaultAvatarPink";
                        editAvatarpic = "DefaultAvatarPinkEdit";
                        break;
                }

                imgUser.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{avatarPic}.png"));
                GameManager.User.UserAvatarPic = avatarPic;
                GameManager.User.UserAvatarEditPic = editAvatarpic;
                GameManager.UpdateData();//saves data in database

                EditAvatarGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgLogout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            imgLogout.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Buttons/LogoutButtonSelected.png"));
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgLogout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            imgLogout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/LogoutButton.png"));
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound and opens LogOutGrid and closes every other grid.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgLogout_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            SettingsGrid.Visibility = Visibility.Collapsed;
            EditAvatarGrid.Visibility = Visibility.Collapsed;
            exitGrid.Visibility = Visibility.Collapsed;

            if (logOutGrid.Visibility == Visibility.Collapsed)
                logOutGrid.Visibility = Visibility.Visible;
            else
                logOutGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyLogout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgVerifyLogout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_Yes.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyLogout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgVerifyLogout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/Yes.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Changes to SignUpPage Frame.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgVerifyLogout_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            GameManager.UpdateData(); //updates user data before logging out.
            Frame.Navigate(typeof(SignUpPage));
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelLogout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            imgCancelLogout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/_No.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelLogout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            imgCancelLogout.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/No.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// collapsing the logOutGrid.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgCancelLogout_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            logOutGrid.Visibility = Visibility.Collapsed;
        }
    }
}
