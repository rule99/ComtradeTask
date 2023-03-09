namespace BackEndAPI.Model.BO
{
    public class CustomerBO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birth { get; set; }

        public string SSN { get; set; }

        public int ReturnCustomer { get; set; }
    }
}
