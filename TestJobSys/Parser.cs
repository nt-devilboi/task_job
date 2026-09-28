using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace TestJobSys;

public partial class Parser(IDocument document) : IDisposable
{
    private static readonly Regex EmailRegex = MyRegex();

    public (int total, List<string> attrs) FindAttributes(string selector, string attribute)
    {
        var elements = document.QuerySelectorAll(selector);

        var attrs = elements
            .Select(e => e.GetAttribute(attribute))
            .Where(v => v is not null)
            .Select(v => v!)
            .ToList();

        return (elements.Count, attrs);
    }

    public List<string> FindEmails()
    {
        var html = document.DocumentElement.OuterHtml;

        return EmailRegex
            .Matches(html)
            .Select(m => m.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    
    
    [GeneratedRegex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    public void Dispose()
    {
        document.Dispose();
    }
}