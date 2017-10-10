using System;

namespace UpShop.Dominio.Helpers
{
    /// <summary>
    /// Store the collection name associate a entity in database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        private string collection;

        /// <summary>
        /// Name of the collection in database.
        /// </summary>
        public string Name
        {
            get { return collection; }
            set { collection = value; }
        }

        public CollectionAttribute(string collectionName)
        {
            collection = collectionName;
        }
    }

}
