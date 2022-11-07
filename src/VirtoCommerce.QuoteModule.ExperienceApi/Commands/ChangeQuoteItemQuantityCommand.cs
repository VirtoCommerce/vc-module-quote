namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteItemQuantityCommand : QuoteCommand
{
    public string LineItemId { get; set; }
    public int Quantity { get; set; }
}

public class ChangeQuoteItemQuantityCommandType : QuoteCommandType<ChangeQuoteItemQuantityCommand>
{
    public ChangeQuoteItemQuantityCommandType()
    {
        Field(x => x.LineItemId);
        Field(x => x.Quantity);
    }
}
