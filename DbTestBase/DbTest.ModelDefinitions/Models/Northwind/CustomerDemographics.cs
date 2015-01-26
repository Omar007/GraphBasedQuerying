namespace DbTest.ModelDefinitions.Models.Northwind
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CustomerDemographics
    {
        public CustomerDemographics()
        {
            Customers = new HashSet<Customers>();
        }

        [Key]
        [StringLength(10)]
        public string CustomerTypeID { get; set; }

        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }
}
