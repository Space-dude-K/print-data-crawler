using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// Organization entity.
    /// </summary>
    [Table("room_history")]
    public class RoomHistory
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrinterId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; }
    }
}