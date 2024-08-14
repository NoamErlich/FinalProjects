using DatabaseProject;
using DatabaseProject.Models;
using GameEngine.GameServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
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
    public sealed partial class SignUpPage : Page
    {
        /// <summary>
        /// Initializing the page.
        /// </summary>
        public SignUpPage()
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
        private void register_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            registerTextBlock.Foreground = new SolidColorBrush(Colors.Aqua);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void register_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            registerTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound, closing SignInWindow, opening RegisterWindow, and 
        /// removing all data in SignInWindow boxes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void register_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            SignInWindow.Visibility = Visibility.Collapsed;
            RegisterWindow.Visibility = Visibility.Visible;

            usernameORemail.Text = "";
            Password.Password = "";
            ErrorMsgsLogin.Text = "";
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void acceptRegister_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            acceptRegisterTextBlock.Foreground = new SolidColorBrush(Colors.Aqua);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void acceptRegister_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            acceptRegisterTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler triggered when the registration acceptance button is pressed.
        /// Plays a clicking sound effect. Checks if the username, password, verify password,
        /// and email fields are empty. If any field is empty, displays an error message 
        /// indicating missing data. If the passwords do not match, displays an error message 
        /// indicating non-identical passwords. If the password is not strong enough, displays
        /// an error message indicating that a stronger password is required. If the email is 
        /// not valid, displays an error message indicating an invalid. If all fields are filled 
        /// correctly and the user does not already exist in the database, adds the user to the 
        /// database, clears any previous error messages, displays a success message indicating 
        /// successful sign-up, and navigates to the MenuPage. If the username or email is already
        /// used, displays an error message indicating that the username or email is already in use.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private async void acceptRegister_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");

            if (Register_userName.Text == "" || Register_Password.Password == "" || Register_verifyPassword.Password == ""
                || Register_email.Text == "") //if all fields is not filled
            {
                ErrorMsgsRegister.Text = "*Data is Missing*";

            }

            else if (!Check2Passwords(Register_Password.Password, Register_verifyPassword.Password)) //if password is not strong enough
            {
                ErrorMsgsRegister.Text = "*Passwords are not identical*";
            }

            else if (!CheckStrongPassword(Register_Password.Password)) //if password is not strong enough
            {
                ErrorMsgsRegister.Text = "*Password is not strong enough, pls choose a strogner password*";

            }

            else if (!CheckValidateEmail(Register_email.Text)) //if password is not strong enough
            {
                ErrorMsgsRegister.Text = "*Email is not valid or already used*";
            }

            else
            {
                string userName = Register_userName.Text.Trim();
                string userPassword = Register_Password.Password.Trim();
                string userEmail = Register_email.Text.Trim();

                int? userId = Server.ValidateUserRegister(userName, userEmail);
                if (userId == null) //user isn't in data base, add him to the data base
                {
                    //adding user to the data base
                    GameManager.User = Server.AddNewUser(userName, userPassword, userEmail);
                    ErrorMsgsRegister.Text = "";
                    await new MessageDialog("", $"Congratulations {userName} you have signed up successfully!").ShowAsync();
                    Frame.Navigate(typeof(MenuPage));
                }
                else
                    ErrorMsgsRegister.Text = "*Username or email is already used, please try a diffrent email or name*";
                //await new MessageDialog("Username or email is already used, please try a diffrent email or name.").ShowAsync();

            }
        }

        /// <summary>
        /// Comparing the 2 passwords value to check if they equal.
        /// </summary>
        /// <param name="password1">Password from passwordBox1</param>
        /// <param name="password2">Password from passwordBox2</param>
        /// <returns>Returns "true" if they equal; else returns "false";</returns>
        private bool Check2Passwords(string password1, string password2)
        {
            return password1 == password2;
        }

        /// <summary>
        /// Checking if the email player used is validate.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>if its validate, returns "true"; else returns "false"</returns>
        private bool CheckValidateEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try //trimmedEmail could be "null", so its important to use Try Catch methods to avoid crashs, incase of string == "null".
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if password is strong enough.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Method returns "true" if password is strong enough if its meets the conditions. If not, returns "false".</returns>
        private bool CheckStrongPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(ch => !char.IsLetter(ch) && !char.IsDigit(ch));
        }


        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void acceptSignIn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            acceptSignInTextBlock.Foreground = new SolidColorBrush(Colors.Aqua);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void acceptSignIn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            acceptSignInTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler triggered when the sign-in acceptance button is pressed. 
        /// Plays a clicking sound effect. Checks if the username or email and password
        /// fields are empty. If either field is empty, displays an error message indicating missing data.
        /// If both fields are filled, attempts to validate if the user's login is valid.
        /// If the user exists, retrieves the user's statistics and information, clears any previous error 
        /// messages, displays a success message welcoming the user, and navigates to the MenuPage.
        /// If the user does not exist, displays an error message indicating that the user does not exist.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private async void acceptSignIn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");

            if (usernameORemail.Text == "" || Password.Password == "")
            {
                //await new MessageDialog("Data is Missing.", "Input Error").ShowAsync();
                ErrorMsgsLogin.Text = "*Data is Missing*";
            }   
            else
            {
                int? userId = Server.ValidateUserLogin(usernameORemail.Text.Trim(), Password.Password.Trim());
                if (userId != null) //UserExist
                {
                    GameManager.User = Server.GetUser(userId.Value); //get user stats and info.
                    ErrorMsgsLogin.Text = "";
                    await new MessageDialog("", $"Successfully logged in, Welcome back {usernameORemail.Text}.").ShowAsync();
                    Frame.Navigate(typeof(MenuPage)); // Navigate to MenuPage.
                }
                else
                    ErrorMsgsLogin.Text = "*User does not exist*";


            }

        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void signIn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            signInTextBlock.Foreground = new SolidColorBrush(Colors.Aqua);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void signIn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            signInTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound, closing RegisterWindow, opening SignInWindow, and 
        /// removing all data in register boxes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void signIn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            RegisterWindow.Visibility = Visibility.Collapsed;
            SignInWindow.Visibility = Visibility.Visible;
            Register_userName.Text = "";
            Register_email.Text = "";
            Register_Password.Password = "";
            Register_verifyPassword.Password = "";
            ErrorMsgsRegister.Text = "";
        }

        /// <summary>
        /// Event handler for when the pointer enters the image.
        /// Changes image, plays sound, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgPasswordInfo_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("PointerEnteredSFX.wav");
            InfoBoxWindow.Visibility = Visibility.Visible;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Event handler for when the pointer exits the image.
        /// Changes image, and changes cursor type.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgPasswordInfo_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            InfoBoxWindow.Visibility = Visibility.Collapsed;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
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
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/LogoutIconButtonGray.png"));
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
            imgExit.Source = new BitmapImage(new Uri("ms-appx:///Assets/Buttons/LogoutIconButtonWhite.png"));
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        /// <summary>
        /// Event handler for when the pointer presses the image.
        /// Makes sound, and exits the game.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void imgExit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SoundPlayer.Play("Click.Wav");
            App.Current.Exit();
        }
    }
}