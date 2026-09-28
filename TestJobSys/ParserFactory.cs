namespace TestJobSys;

using AngleSharp;

public class ParserFactory
{
    private readonly IBrowsingContext _context = BrowsingContext.New(Configuration.Default);

    public async Task<Parser> CreateAsync(string html)
    {
        var document = await _context.OpenAsync(req => req.Content(html));
        return new Parser(document);
    }
}