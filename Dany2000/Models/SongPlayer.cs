using Dany2000.Api;
using Dany2000.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;

namespace Dany2000.Models
{
    public class SongPlayer : ISongPlayer
    {
        private readonly MediaPlayer _player;

        public event EventHandler SongEnded;
        public event EventHandler<TimeSpan> EllapsedTimeChange;

        private DispatcherTimer _timer = new DispatcherTimer();

        public TimeSpan SongDuration => _player.NaturalDuration.TimeSpan;

        public SongPlayer()
        {
            _player = new MediaPlayer();
            _player.MediaEnded += NotifyEndOfSong;

            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(DisplayEllapsedTime);
         
        }

        void DisplayEllapsedTime(object sender, EventArgs e)
        {
            var tempsRestant = _player.NaturalDuration.TimeSpan - _player.Position;
            EllapsedTimeChange?.Invoke(this, tempsRestant);
        }

        private void NotifyEndOfSong(object sender, EventArgs e)
        {
            SongEnded?.Invoke(this, EventArgs.Empty);
        }

        public void DecreaseVolume()
        {
            throw new NotImplementedException();
        }

        public void IncreaseVolume()
        {
            throw new NotImplementedException();
        }

        public void MuteVolume()
        {
            throw new NotImplementedException();
        }

        public void Pause(Song song)
        {
            throw new NotImplementedException();
        }

        public void Play(Song song)
        {
            if (_player.Source != null)
                _player.Close();

            _player.Open(new Uri(song.MusicFilePath));
            _player.Play();
            _timer.Start();
        }

        public void Stop()
        {
            _player.Stop();
            _timer.Stop();
        }
    }
}
