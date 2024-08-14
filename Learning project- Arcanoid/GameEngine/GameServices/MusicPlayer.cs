using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Web.Syndication;

namespace GameEngine.GameServices
{
    /// <summary>
    /// Static class for playing music.
    /// </summary>
    public static class MusicPlayer
    {
        private static MediaPlayer _mediaPlayer = new MediaPlayer(); // The media player
        public static bool IsOn = false; // Indicates if the music player is on
        private static double _volume = 0.5; // The volume level

        /// <summary>
        /// Plays the specified music file.
        /// </summary>
        /// <param name="fileName">The name of the music file to play.</param>
        public static void Play(string fileName)
        {
            if (!IsOn)
            {
                IsOn = true;
                _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($"ms-appx:///Assets/Music/{fileName}"));
                _mediaPlayer.IsLoopingEnabled = true;
                _mediaPlayer.Play();
            }
        }

        /// <summary>
        /// Plays a random music file from the specified list.
        /// </summary>
        /// <param name="fileNames">The array of music file names.</param>
        public static void PlayRandom(string[] fileNames)
        {
            Random rnd = new Random();
            int i = rnd.Next(fileNames.Length);
            Play(fileNames[i]);
        }

        /// <summary>
        /// Stops playing the music.
        /// </summary>
        public static void Stop()
        {
            IsOn = false;
            _mediaPlayer.Pause();
        }

        /// <summary>
        /// Gets or sets the volume level of the music player.
        /// </summary>
        public static double Volume
        {
            set
            {
                _volume = value / 100;
                _mediaPlayer.Volume = _volume;
            }

            get
            {
                return _volume * 100;
            }
        }

        /// <summary>
        /// Gets the current volume level of the music player.
        /// </summary>
        /// <returns>The current volume level.</returns>
        public static double GetVolume()
        {
            return _mediaPlayer.Volume;
        }
    }
}
