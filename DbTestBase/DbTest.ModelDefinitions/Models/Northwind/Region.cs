namespace DbTest.ModelDefinitions.Models.Northwind
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Region")]
    public class Region
    {
        public Region()
        {
            Territories = new HashSet<Territories>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegionID { get; set; }

        [Required]
        [StringLength(50)]
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}
