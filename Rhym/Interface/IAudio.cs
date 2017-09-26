using System;
namespace Rhym
{
    public interface IAudio
    {
        bool Play_Pause(string url);
        bool Stop(bool val);
    }
}
