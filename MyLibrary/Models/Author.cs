using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Author
    {
        [Display(Name = "Author Id")]
        public int AuthorID { get; set; }

        [StringLength(50)]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string FullName { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}
