namespace UpShop.Api.Utils
{
    interface IMapperEntity<T>
    {
        /// <summary>
        /// Mappers properties of a DTO to a entity by reference.
        /// </summary>
        /// <param name="entity"></param>
        void MapperProperties(T entity);
    }
}
