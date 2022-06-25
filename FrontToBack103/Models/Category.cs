using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack103.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="don't empty"),MaxLength(10,ErrorMessage = "No more than 10 characters")]
        public string Name { get; set; }

   
        [Required(ErrorMessage ="dont empty"),MaxLength(50, ErrorMessage = "No more than 50 characters")] 
        public string Desc { get; set; }


        public virtual IEnumerable<Product> Products { get; set; }
    }
}
