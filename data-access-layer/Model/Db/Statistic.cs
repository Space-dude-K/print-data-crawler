using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// Printer statistics entity.
    /// </summary>
    [Table("statistic")]
    public class Statistic
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int Id { get; set; }
        public int TonerLevel { get; set; }
        public int DrumLevel { get; set; }
        public int TotalPagesPrinted { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Printer Printer { get; set; }
    }
}