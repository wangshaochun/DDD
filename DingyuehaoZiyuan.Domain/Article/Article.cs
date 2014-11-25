using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DingyuehaoZiyuan.Domain
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public int Id{ get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
    }
}
