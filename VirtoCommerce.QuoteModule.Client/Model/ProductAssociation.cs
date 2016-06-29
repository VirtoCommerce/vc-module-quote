using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.QuoteModule.Client.Model
{
    /// <summary>
    /// ProductAssociation
    /// </summary>
    [DataContract]
    public partial class ProductAssociation :  IEquatable<ProductAssociation>
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets AssociatedObjectId
        /// </summary>
        [DataMember(Name="associatedObjectId", EmitDefaultValue=false)]
        public string AssociatedObjectId { get; set; }

        /// <summary>
        /// Gets or Sets AssociatedObjectType
        /// </summary>
        [DataMember(Name="associatedObjectType", EmitDefaultValue=false)]
        public string AssociatedObjectType { get; set; }

        /// <summary>
        /// Gets or Sets AssociatedObject
        /// </summary>
        [DataMember(Name="associatedObject", EmitDefaultValue=false)]
        public Entity AssociatedObject { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProductAssociation {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  AssociatedObjectId: ").Append(AssociatedObjectId).Append("\n");
            sb.Append("  AssociatedObjectType: ").Append(AssociatedObjectType).Append("\n");
            sb.Append("  AssociatedObject: ").Append(AssociatedObject).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as ProductAssociation);
        }

        /// <summary>
        /// Returns true if ProductAssociation instances are equal
        /// </summary>
        /// <param name="other">Instance of ProductAssociation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProductAssociation other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.AssociatedObjectId == other.AssociatedObjectId ||
                    this.AssociatedObjectId != null &&
                    this.AssociatedObjectId.Equals(other.AssociatedObjectId)
                ) && 
                (
                    this.AssociatedObjectType == other.AssociatedObjectType ||
                    this.AssociatedObjectType != null &&
                    this.AssociatedObjectType.Equals(other.AssociatedObjectType)
                ) && 
                (
                    this.AssociatedObject == other.AssociatedObject ||
                    this.AssociatedObject != null &&
                    this.AssociatedObject.Equals(other.AssociatedObject)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.AssociatedObjectId != null)
                    hash = hash * 59 + this.AssociatedObjectId.GetHashCode();

                if (this.AssociatedObjectType != null)
                    hash = hash * 59 + this.AssociatedObjectType.GetHashCode();

                if (this.AssociatedObject != null)
                    hash = hash * 59 + this.AssociatedObject.GetHashCode();

                return hash;
            }
        }
    }
}
