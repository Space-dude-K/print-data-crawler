using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace data_access_layer.Model.Db
{
    /// <summary>
    /// City entity.
    /// </summary>
    [Table("city")]
    public class City
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
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
