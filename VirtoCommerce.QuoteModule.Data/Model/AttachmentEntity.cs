using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

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
        
        public string QuoteRequestId { get; set; }
        public virtual QuoteRequestEntity QuoteRequest { get; set; }

        #endregion
       
        public virtual AttachmentEntity ToModel(AttachmentEntity attachment)
        {
          if (attachment == null)
            throw new ArgumentNullException(nameof(attachment));

          attachment.Id = this.Id;
          attachment.Url = this.Url;
          attachment.Name = this.Name;
          attachment.MimeType = this.MimeType;
          attachment.Size = this.Size;
          attachment.QuoteRequestId = this.QuoteRequestId;
       
          return attachment;
        }
       
        public virtual AttachmentEntity FromModel(AttachmentEntity attachment)
        {
          if (attachment == null)
            throw new ArgumentNullException(nameof(attachment));
       
          this.Id = attachment.Id;
          this.Url = attachment.Url;
          this.Name = attachment.Name;
          this.MimeType = attachment.MimeType;
          this.Size = attachment.Size;
          this.QuoteRequestId = attachment.QuoteRequestId;
       
          return this;
        }
       
        public virtual void Patch(AttachmentEntity target)
        {
          target.Url = this.Url;
          target.Name = this.Name;
          target.MimeType = this.MimeType;
          target.Size = this.Size;
        }       
    }
}
