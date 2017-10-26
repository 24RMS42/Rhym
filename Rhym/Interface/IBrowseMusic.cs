using System;
using System.Collections.ObjectModel;

namespace Rhym
{
    public interface IBrowseMusic
    {
        bool BrowseMusic();
        ObservableCollection<SongModel> GetLocalMusic();
    }
}
