using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingyuehaoZiyuan.Domain
{
    [Table("Author")]
    public class Author
    {
        [Key]
        public Guid AuthorID { get; set; }
        public string AuthorName { get; set; }
    }
}
