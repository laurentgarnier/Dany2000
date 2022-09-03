using Dany2000.Api;
using Dany2000.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Threading;

namespace Dany2000.ViewModels
{
    internal class DisplayViewModel : BindableBase
    {
        private readonly IPlaylistManager _playlistManager;
        private readonly ISongPlayer _songPlayer;
        private readonly IEventAggregator _eventAggregator;


        private string _pdfFile;

        public DelegateCommand WindowClosingCommand { get; private set; }

        public string PdfFile
        {
            get => _pdfFile;
            set
            {
                SetProperty(ref _pdfFile, value);
            }
        }

        public DisplayViewModel(ISongPlayer songPlayer, IPlaylistManager playlistManager, IEventAggregator eventAggregator)
        {
            _playlistManager = playlistManager;
            _songPlayer = songPlayer;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<RequestPlayingSongEvent>().Subscribe(PlaySong);
            _eventAggregator.GetEvent<StopSongEvent>().Subscribe(StopSong);
            _eventAggregator.GetEvent<WindowClosingEvent>().Subscribe(() => Process.GetCurrentProcess().Kill());

            _songPlayer.SongEnded += ManageEndOfSong;
            _songPlayer.EllapsedTimeChange += _songPlayer_EllapsedTimeChange;
            WindowClosingCommand = new DelegateCommand(WindowClosing);
        }

        private string _musicPosition;
        public string MusicPosition
        {
            get => _musicPosition;
            set
            {
                SetProperty(ref _musicPosition, value);
            }
        }

        private void _songPlayer_EllapsedTimeChange(object sender, TimeSpan tempsRestant)
        {
            
            MusicPosition = $"{tempsRestant.ToString("mm\\:ss")} / {_songPlayer.SongDuration.ToString("mm\\:ss")}";
            ManageLastXSeconds(10, tempsRestant);
        }

        private Brush _blinkColor;

        public Brush BlinkColor
        {
            get => _blinkColor;
            set
            {
                SetProperty(ref _blinkColor, value);    
            }
        }

        private Brush _defaultBackColor = Brushes.White;
        private Brush _warningBackColor = Brushes.Red;

        private void ManageLastXSeconds(int nbSeconds, TimeSpan tempsRestant)
        {
            if (tempsRestant < TimeSpan.FromSeconds(nbSeconds))
            {
                if (_blinkColor == _defaultBackColor)
                    BlinkColor = _warningBackColor;
                else
                    BlinkColor = _defaultBackColor;
            }
        }


        private void StopSong()
        {
            _songPlayer.Stop(); ;
        }

        private void WindowClosing()
        {
            _eventAggregator.GetEvent<WindowClosingEvent>().Publish();
        }

        private void ManageEndOfSong(object sender, EventArgs e)
        {
            _eventAggregator.GetEvent<SongEndedEvent>().Publish();
        }

        private void PlaySong(Song song)
        {
            _blinkColor = _defaultBackColor;
            _songPlayer.Play(song);
            PdfFile = song.LyricsFilePath;
            _playlistManager.Remove(song);
        }
    }
}
