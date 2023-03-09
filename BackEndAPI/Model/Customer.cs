using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndAPI.Model
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birth { get; set; }

        public string SSN { get; set; }

        public int ReturnCustomer { get; set; }

        public DateTime DateRewarded { get; set; }

        public string AgentUserName { get; set; }
    }
}
