using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace GameEngine.GameServices
{
    /// <summary>
    /// Static class for playing sound effects.
    /// </summary>
    public static class SoundPlayer
    {
        public static MediaPlayer _mediaPlayer = new MediaPlayer(); // The media player
        public static bool IsOn { get; set; } = true; // Gets or sets whether the sound player is on.

        private static double _volume = 0.5; // The volume level

        /// <summary>
        /// Gets or sets the volume level of the sound player.
        /// </summary>
        public static double Volume
        {
            get
            {
                return _volume * 100;
            }
            set
            {
                _volume = value / 100;
                _mediaPlayer.Volume = _volume;
            }
        }


        /// <summary>
        /// Plays the specified sound effect file.
        /// </summary>
        /// <param name="fileName">The name of the sound effect file to play.</param>
        public static void Play(string fileName)
        {
            if (IsOn)
            {
                _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($"ms-appx:///Assets/SFX/{fileName}"));
                _mediaPlayer.Play();
            }
        }

        /// <summary>
        /// Stops playing the current sound effect.
        /// </summary>
        public static void Stop()
        {
            _mediaPlayer.Pause();
        }
    }
}
