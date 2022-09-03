using System;
using System.Collections.Generic;
using System.Text;

namespace Dany2000.Api
{
    public interface IPlaylistManager
    {
        public IEnumerable<Song> Songs { get; }

        public void AddSong(Song song); 

        void Remove(Song song);
        void GotoNextSong();

        public event EventHandler<Song> SongChanged;
        public event EventHandler SongListChanged;
    }
}
