namespace EtbSomalia.Models
{
    public class DrugReceiptDetails
    {
        public long Id { get; set; }
        public DrugReceipt Receipt { get; set; }
        public DrugIssue Issue { get; set; }
        public DrugBatches Batch { get; set; }
        public long Quantity { get; set; }
        public string Description { get; set; }

        public DrugReceiptDetails() {
            Id = 0;
            Quantity = 0;
            Description = "";
            Issue = new DrugIssue();
            Batch = new DrugBatches();
            Receipt = new DrugReceipt();
        }
    }
}
