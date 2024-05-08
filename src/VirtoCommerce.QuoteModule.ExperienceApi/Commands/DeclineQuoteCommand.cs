namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeclineQuoteCommand : QuoteCommand
{
    public string Comment { get; set; }
}

public class DeclineQuoteCommandType : QuoteCommandType<DeclineQuoteCommand>
{
    public DeclineQuoteCommandType()
    {
        Field(x => x.Comment);
    }
}
