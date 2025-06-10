using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models;

public class QuoteConfigurationItemFile : AuditableEntity, ICloneable
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string ConfigurationItemId { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
