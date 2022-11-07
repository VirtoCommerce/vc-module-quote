namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteCommentCommand : QuoteCommand
{
    public string Comment { get; set; }
}

public class ChangeQuoteCommentCommandType : QuoteCommandType<ChangeQuoteCommentCommand>
{
    public ChangeQuoteCommentCommandType()
    {
        Field(x => x.Comment);
    }
}
