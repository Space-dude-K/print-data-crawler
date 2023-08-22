using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// PrinterType entity.
    /// </summary>
    [Table("type")]
    public class Type
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual Printer Printer { get; set; }
    }
}