using System;

namespace picture_Backend.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();

    }
}
