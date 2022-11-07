namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class RemoveQuoteItemCommand : QuoteCommand
{
    public string LineItemId { get; set; }
}

public class RemoveQuoteItemCommandType : QuoteCommandType<RemoveQuoteItemCommand>
{
    public RemoveQuoteItemCommandType()
    {
        Field(x => x.LineItemId);
    }
}
