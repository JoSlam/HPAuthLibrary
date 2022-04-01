namespace HealthPass.Data.Entities
{
    public class BlockedSignature
    {
        public int ID { get; set; }
        public string Signature { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUTC { get; set; }
        public DateTime ExpiryDateUTC { get; set; }
    }
}
