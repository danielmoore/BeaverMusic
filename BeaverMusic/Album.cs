using System.Collections.Generic;

namespace BeaverMusic
{
    public class Album
    {
        public Album() { }

        public Album(int id)
        {
            Id = id;
        }

        public int? Id { get; internal set; }

        public string Name { get; set; }

        public string Artist { get; set; }

        public IEnumerable<Song> Songs { get; set; }
    }
}
