namespace BrunoTheBot.CoreBusiness.Entities.PDFData
{
    public class PDFDataPages
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Page { get; set; }
        public string Content { get; set; } = "";
        public string Title { get; set; } = "";
    }

    public enum PDFCategory
    {
        Cover,
        TitlePage,
        CopyrightPage,
        Dedication,
        Acknowledgments,
        TableOfContents,
        Foreword,
        Preface,
        Introduction,
        Body,
        Conclusion,
        Appendices,
        Glossary,
        ReferencesOrBibliography,
        Index,
        AboutTheAuthor
    }
}
