using TestJobSys.Db;

namespace TestJobSys;

public class UseCasesSearch(ParserFactory parserFactory, ElementRepository elementsRepository)
{
    public async Task<CheckResponse?> Search(CheckRequest request)
    {
        var pageHtml = Decoder.DecodeBase64(request.PageB64);
        var url = Decoder.DecodeBase64(request.UrlB64);

        using var parser = await parserFactory.CreateAsync(pageHtml);

        var attrs = parser.FindAttributes(request.Selector, request.Attribute);
        await elementsRepository.AddRangeAsync(attrs.attrs
            .Select(x => new Element() { AttributeValue = x, OuterHtml = pageHtml }));


        var emails = parser.FindEmails();
        var decrypted = Decoder.DecryptAesEcb(request.EncryptedTextBytesB64, request.KeyBytesB64);
        return new CheckResponse(0, null, null, attrs.total, emails.Count, url, decrypted, attrs.attrs, emails);
    }
}

public record CheckResponse(
    int IsError,
    string? ErrorCode,
    string? ErrorMessage,
    int ElementsCount,
    int EmailsCount,
    string? Url,
    string? DecryptedPlainText,
    List<string> ElementsAttrList,
    List<string> EmailsList
)
{
    public static CheckResponse Fail(string errorCode, string errorMessage)
    {
        return new CheckResponse(1, errorCode, errorMessage, 0, 0, null, null, [], []);
    }
};