using Dany2000.Api;
using Dany2000.Models;
using Dany2000.Views;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Dany2000.ViewModels
{
    internal class ManagementViewModel : BindableBase
    {
        private ObservableCollection<string> _availableSongs;
        private string _selectedSongInAvailableSongs;

        private string _defaultRootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

        private string _rootDirectory;

        private readonly ISongsManager _songsManager;
        private readonly IPlaylistManager _playlistManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly MvvmDialogs.IDialogService _dialogService;
        private ICollectionView _songsView;
        private string _filterText;

        public DelegateCommand SongListDoubleClick { get; private set; }

        public DelegateCommand PlaySongCommand { get; private set; }
        public DelegateCommand NextSongCommand { get; private set; }
        public DelegateCommand RemoveSongCommand { get; private set; }
        public DelegateCommand OpenRootDirectoryCommand { get; private set; }
        public DelegateCommand WindowClosingCommand { get; private set; }
        public DelegateCommand StopSongCommand { get; private set; }



        public ManagementViewModel(ISongsManager songsManager, IPlaylistManager playlistManager, IEventAggregator eventAggregator, MvvmDialogs.IDialogService dialogService, DisplayWindow displayWindow)
        {
            _songsManager = songsManager;
            _playlistManager = playlistManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _rootDirectory = _defaultRootDirectory;
            _songsManager.Scan(_rootDirectory);
            FillAvailableSongs();

            _songsView = CollectionViewSource.GetDefaultView(AvailableSongs);
            _songsView.Filter = o => string.IsNullOrEmpty(FilterText) || ((string)o).Contains(FilterText);

            _playlistManager.SongListChanged += RefreshPlayList;
            _playlistManager.SongChanged += LoadSong;

            _eventAggregator.GetEvent<SongEndedEvent>().Subscribe(ManageEndOfSong);
            _eventAggregator.GetEvent<WindowClosingEvent>().Subscribe(() => Process.GetCurrentProcess().Kill());

            SetCommands();
            displayWindow.Show();
        }

        private void FillAvailableSongs()
        {
            if (AvailableSongs == null)
                AvailableSongs = new ObservableCollection<string>();

            AvailableSongs.Clear();
            AvailableSongs.AddRange(_songsManager.Songs.Select(song => song.Name).ToList());
        }

        private void ManageEndOfSong()
        {
            if (_playlistManager.Songs.Count() == 0) return;
            _playlistManager.Remove(_playlistManager.Songs.First());
            _playlistManager.GotoNextSong();
        }

        #region Propriétés bindées
        /// <summary>
        /// Liste des chansons possibles
        /// </summary>
        public ObservableCollection<string> AvailableSongs
        {
            get => _availableSongs;
            set
            {
                SetProperty(ref _availableSongs, value);
            }
        }

        ///// <summary>
        ///// Nom de la chanson sélectionnée dans la liste des chansons
        ///// </summary>
        public string SelectedSongInAvailableSongs
        {
            get => _selectedSongInAvailableSongs;
            set
            {
                SetProperty(ref _selectedSongInAvailableSongs, value);
            }
        }

        public ObservableCollection<Song> PlaylistSongs { get => _playlistSongs; }

        private ObservableCollection<Song> _playlistSongs = new ObservableCollection<Song>();

        private string _songInProgress;

        public string SongInProgress { get => _songInProgress; set => SetProperty(ref _songInProgress, value); }

        private int _playlistSelectedIndex;

        public int PlaylistSelectedIndex { get => _playlistSelectedIndex; set => SetProperty(ref _playlistSelectedIndex, value); }

        private string _pdfFile;

        public string PdfFile { get => _pdfFile; set => SetProperty(ref _pdfFile, value); }

        #endregion Propriétés bindées


        #region commandes
        private void SetCommands()
        {
            OpenRootDirectoryCommand = new DelegateCommand(OpenRootDirectory);
            PlaySongCommand = new DelegateCommand(PlaySong);
            SongListDoubleClick = new DelegateCommand(AddSongToPlaylist);
            NextSongCommand = new DelegateCommand(GotoNextSong);
            StopSongCommand = new DelegateCommand(StopSong);
            RemoveSongCommand = new DelegateCommand(RemoveSong);
            WindowClosingCommand = new DelegateCommand(WindowClosing);
        }

        private void StopSong()
        {
            _eventAggregator.GetEvent<StopSongEvent>().Publish();
        }

        private void GotoNextSong()
        {
            _playlistManager.Remove(_playlistManager.Songs.First());
            _playlistManager.GotoNextSong();
        }

        private void WindowClosing()
        {
            _eventAggregator.GetEvent<WindowClosingEvent>().Publish();
        }

        private void OpenRootDirectory()
        {
            var settings = new FolderBrowserDialogSettings
            {
                Description = "Sélectionner le répertoire contenant les fichiers",
                SelectedPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!
            };

            bool? success = _dialogService.ShowFolderBrowserDialog(this, settings);
            if (success == true)
            {
                _rootDirectory = settings.SelectedPath;
                _songsManager.ClearSongs();
                _songsManager.Scan(_rootDirectory);
                FillAvailableSongs();
            }
        }

        private void RemoveSong()
        {
            if (_playlistSelectedIndex == -1) return;

            _playlistManager.Remove(_playlistManager.Songs.ElementAt(_playlistSelectedIndex));
        }

        private void PlaySong()
        {
            if (_playlistManager.Songs.Count() <= 0) return;

            PlaySong(_playlistManager.Songs.First());
        }

        private void AddSongToPlaylist()
        {
            _playlistManager.AddSong(_songsManager.Songs.First(song => song.Name.Equals(_selectedSongInAvailableSongs)));
        }
        #endregion commandes

        /// <summary>
        /// Texte saisi dans la zone de filtre
        /// </summary>
        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                if (value != _filterText)
                {
                    SetProperty(ref _filterText, value);
                    _songsView.Refresh();
                }
            }
        }

        private void RefreshPlayList(object sender, EventArgs e)
        {
            PlaylistSongs.Clear();
            PlaylistSongs.AddRange<Song>(_playlistManager.Songs);
        }

        private void LoadSong(object sender, Song song)
        {
            PlaySong(song);
        }

        private void PlaySong(Song song)
        {
            _eventAggregator.GetEvent<RequestPlayingSongEvent>().Publish(song);
            SongInProgress = song.Name;
            PdfFile = song.LyricsFilePath;
        }
    }
}
