namespace BrunoTheBot.CoreBusiness.Entities.PDFData
{
    public class PDFDataPages
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Page { get; set; }
        public string Content { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
