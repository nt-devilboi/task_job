using FluentValidation;

namespace TestJobSys;

public class ValidatorJson : AbstractValidator<CheckRequest>
{
    public ValidatorJson()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Selector)
            .NotEmpty().WithErrorCode("EMPTY_SELECTOR");

        RuleFor(x => x.Attribute)
            .NotEmpty().WithErrorCode("EMPTY_ATTRIBUTE");

        RuleFor(x => x.UrlB64)
            .NotEmpty().WithErrorCode("EMPTY_URL_B64")
            .Must(BeValidBase64).WithErrorCode("INVALID_BASE64_URL");

        RuleFor(x => x.PageB64).NotEmpty().WithErrorCode("EMPTY_PAGE_B64")
            .Must(BeValidBase64).WithErrorCode("INVALID_BASE64_PAGE");

        RuleFor(x => x.EncryptedTextBytesB64)
            .NotEmpty().WithErrorCode("EMPTY_ATTRIBUTE_ENCRYPTED_TEXT")
            .Must(BeValidBase64).WithErrorCode("INVALID_BASE64_ENCRYPTED_TEXT");

        RuleFor(x => x.KeyBytesB64)
            .NotEmpty().WithErrorCode("EMPTY_ATTRIBUTE_KEY_BYTES")
            .Must(BeValidBase64).WithErrorCode("INVALID_BASE64_KEY_BYTES");
    }

    private static bool BeValidBase64(string value) =>
        Convert.TryFromBase64String(value, new byte[value.Length], out _);
}