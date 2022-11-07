namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CancelQuoteCommand : QuoteCommand
{
    public string Comment { get; set; }
}

public class CancelQuoteCommandType : QuoteCommandType<CancelQuoteCommand>
{
    public CancelQuoteCommandType()
    {
        Field(x => x.Comment);
    }
}
