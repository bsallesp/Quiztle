namespace BrunoTheBot.CoreBusiness.Entities.PDFData
{
    public class PDFData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string FileName { get; set; } = "";
        public List<PDFDataPages> Pages { get; set; } = [];
        public string Description { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
