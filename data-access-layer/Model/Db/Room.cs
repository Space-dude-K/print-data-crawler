using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// Organization entity.
    /// </summary>
    [Table("room")]
    public class Room
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
        public virtual Organization Organization { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual RoomHistory RoomHistory { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ICollection<Printer> Printers { get; set; }
    }
}