using MongoDB.Bson;
using System;

namespace UpShop.Dominio.Interfaces
{
    /// <summary>
    /// Shared information of every entity in datebase
    /// </summary>
    public interface IEntity
    {
        ObjectId Id { get; set; }
        bool IsActive { get; set; }
        DateTime DateCriation { get; set; }
        DateTime? DateModification { get; set; }
        string CreateBy { get; set; }
        string ModifyBy { get; set; }
    }

}
