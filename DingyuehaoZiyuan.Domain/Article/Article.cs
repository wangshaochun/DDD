using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DingyuehaoZiyuan.Domain
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Column("Title"), StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// AuthorID
        /// </summary>
        [DisplayName("AuthorID")]
        public Guid AuthorID { get; set; }
        /// <summary>
        /// Author
        /// </summary>
        [ForeignKey("AuthorID")]
        public virtual Author Author { get; set; }
    }
}
