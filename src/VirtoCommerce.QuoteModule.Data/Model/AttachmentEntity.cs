using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class AttachmentEntity : AuditableEntity
    {

        [StringLength(2083)]
        [Required]
        public string Url { get; set; }

        [StringLength(1024)]
        public string Name { get; set; }

        [StringLength(128)]
        public string MimeType { get; set; }

        public long Size { get; set; }


        #region Navigation Properties

        public virtual QuoteRequestEntity QuoteRequest { get; set; }
        public string QuoteRequestId { get; set; }

        #endregion


        public virtual QuoteAttachment ToModel(QuoteAttachment target)
        {
            target.Id = Id;
            target.CreatedBy = CreatedBy;
            target.CreatedDate = CreatedDate;
            target.ModifiedBy = ModifiedBy;
            target.ModifiedDate = ModifiedDate;

            target.Url = Url;
            target.Name = Name;
            target.MimeType = MimeType;
            target.Size = Size;

            return target;
        }

        public virtual AttachmentEntity FromModel(QuoteAttachment item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Id = item.Id;
            CreatedBy = item.CreatedBy;
            CreatedDate = item.CreatedDate;
            ModifiedBy = item.ModifiedBy;
            ModifiedDate = item.ModifiedDate;

            Url = item.Url;
            Name = item.Name;
            MimeType = item.MimeType;
            Size = item.Size;

            return this;
        }

        public virtual void Patch(AttachmentEntity target)
        {
            target.Url = Url;
            target.Name = Name;
            target.MimeType = MimeType;
            target.Size = Size;
        }
    }
}
