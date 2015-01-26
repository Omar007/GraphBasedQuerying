namespace DbTest.ModelDefinitions.Models.Northwind
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Territories
    {
        public Territories()
        {
            Employees = new HashSet<Employees>();
        }

        [Key]
        [StringLength(20)]
        public string TerritoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string TerritoryDescription { get; set; }

        public int RegionID { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
