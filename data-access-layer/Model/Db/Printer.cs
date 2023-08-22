using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// Printer entity.
    /// </summary>
    [Table("printer")]
    public class Printer
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int Id { get; set; }
        [ForeignKey("Type")]
        public int? TypeId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Type Type { get; set; }
        [ForeignKey("Statistic")]
        public int? StatisticId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Statistic Statistic { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Room Room { get; set; }
    }
}
