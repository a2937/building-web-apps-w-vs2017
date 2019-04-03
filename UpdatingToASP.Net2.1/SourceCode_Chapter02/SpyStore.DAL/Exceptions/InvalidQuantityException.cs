using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace SpyStore.DAL.Exceptions
{
    [Serializable]
    public class InvalidQuantityException : Exception
    {
        public string ResourceReferenceProperty { get; set; }


        public InvalidQuantityException()
        {

        }

        public InvalidQuantityException(string message) : base(message)
        {

        }

        public InvalidQuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidQuantityException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
            ResourceReferenceProperty = info.GetString(nameof(ResourceReferenceProperty));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(nameof(ResourceReferenceProperty), ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
