using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Book
    {
        [Display(Name = "Number")]
        public int BookID { get; set; }

        [StringLength(50)]
        public string Title { get; set; }
        
        public Genre Genre { get; set; }

        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        public int AuthorID { get; set; }

        public Author Author { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
    public enum Genre
    {
        Adventure, Novel,Modernism, Romance ,Anthology ,Crime ,Drama ,Fantasy ,Mystery ,Satire
    }
}
