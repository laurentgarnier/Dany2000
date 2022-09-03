using Dany2000.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dany2000.Business
{
    public class PlaylistManager : IPlaylistManager
    {
      //  private Queue<Song> _songs = new Queue<Song>();
        private List<Song> _songsList = new List<Song>();

        public IEnumerable<Song> Songs
        {
            get { return _songsList; }
        }

        public event EventHandler<Song> SongChanged;
        public event EventHandler SongListChanged;

        public void AddSong(Song song)
        {
            if (_songsList.Contains(song)) return;

            _songsList.Add(song);
            SongListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void GotoNextSong()
        {
            if(_songsList.Count == 0) return;
            var song = _songsList[0];
            SongChanged?.Invoke(this, song);
            Remove(song);
        }
        

        public void Remove(Song song)
        {
            _songsList.Remove(song);
            SongListChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
