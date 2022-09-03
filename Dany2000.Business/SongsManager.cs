using Dany2000.Api;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dany2000.Business
{
    public class SongsManager : ISongsManager
    {
        private readonly List<Song> _songs;

        public SongsManager()
        {
            _songs = new List<Song>();
        }

        public IEnumerable<Song> Songs { get => _songs; }

        public void ClearSongs()
        {
            _songs.Clear();
        }

        public void Scan(string songsRootPath)
        {
            var scanner = new DirectoryInfo(songsRootPath);
            var mp3Files = scanner.GetFiles("*.mp3").ToList();
            var pdfFiles = scanner.GetFiles("*.pdf").ToList();

            foreach (var mp3File in mp3Files)
            {
                foreach (var pdfFile in pdfFiles)
                {
                    if(Path.GetFileNameWithoutExtension(mp3File.Name).Equals(Path.GetFileNameWithoutExtension(pdfFile.Name)))
                    {
                        _songs.Add(new Song()
                        {
                            Name = Path.GetFileNameWithoutExtension(mp3File.Name),
                            MusicFilePath = mp3File.FullName,
                            LyricsFilePath = pdfFile.FullName
                        });
                    }
                }
            }
        }
    }
}
