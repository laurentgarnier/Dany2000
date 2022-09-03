using System;

namespace Dany2000.Api
{
    public interface ISongPlayer
    {
        public void Play(Song song);
        public void Stop();
        public void Pause(Song song);
        public void IncreaseVolume();
        public void DecreaseVolume();
        public void MuteVolume();

        public TimeSpan SongDuration { get; }

        event EventHandler SongEnded;
        event EventHandler<TimeSpan> EllapsedTimeChange;

    }
}
