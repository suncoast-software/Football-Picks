using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required(ErrorMessage = "Please Enter Player Name!")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Company Name!")]
        [StringLength(100)]
        public string Company { get; set; }

        public virtual List<Pick> Picks { get; set; }
    }
}
