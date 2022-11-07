namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class SubmitQuoteCommand : QuoteCommand
{
    public string Comment { get; set; }
}

public class SubmitQuoteCommandType : QuoteCommandType<SubmitQuoteCommand>
{
    public SubmitQuoteCommandType()
    {
        Field(x => x.Comment);
    }
}
