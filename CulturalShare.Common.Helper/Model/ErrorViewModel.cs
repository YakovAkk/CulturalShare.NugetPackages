namespace CulturalShare.Common.Helper.Model;

public class ErrorViewModel
{
    public int Status { get; set; }
    public string Error { get; set; }
    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; set; }
}
