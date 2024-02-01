using VirtoCommerce.FileExperienceApi.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Extensions;

public static class FileExtensions
{
    public static bool OwnerIsEmpty(this File file)
    {
        return
            string.IsNullOrEmpty(file.OwnerEntityId) &&
            string.IsNullOrEmpty(file.OwnerEntityType);
    }
    public static bool OwnerIs(this File file, string type, string id)
    {
        return
            file.OwnerEntityId == id &&
            file.OwnerEntityType == type;
    }
}
