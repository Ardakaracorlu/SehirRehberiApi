using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class City
    {
        public City()
        {
            Photos = new List<Photo>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Photo> Photos { get; set; } // Şehrin birden çok resmi vardır 
        public User User { get; set; } // Şehrin ekleyini bir kullanıcı
    }
}
