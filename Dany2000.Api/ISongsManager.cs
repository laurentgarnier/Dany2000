using System.Collections.Generic;

namespace Dany2000.Api
{
    public interface ISongsManager
    {
        public void Scan(string songsRootPath);
        public void ClearSongs();
        public IEnumerable<Song> Songs { get; }

    }
}
