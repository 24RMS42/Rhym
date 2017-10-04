using System;
namespace Rhym
{
    public class SongModel : BaseModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string SongName { get; set; } = "xxx";
        public string ArtistName { get; set; } = "Unknown";
        public string AlbumName { get; set; }
        public int OrderNumber { get; set; }
        public int PopularityRate { get; set; }

        public long MusicId { get; set; }
        public string AlbumId { get; set; }

        bool isselected;
        public bool IsSelected{
            get{
                return isselected;
            }set{
                isselected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(CheckImage));
            }
        }

        public string CheckImage
        {
            get{
                if(IsSelected){
                    return "check";
                }
                return "uncheck";
            }
        }

        public string Url { get; set; } = "";
        public bool IsUrl
        {
            get
            {
                return Url.Contains("http");
            }
        }
    }
}
