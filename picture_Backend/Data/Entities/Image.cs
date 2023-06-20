using System;
using Dapper.Contrib.Extensions;

namespace picture_Backend.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
      

    }
}
