using MongoDB.Bson;
using System;
using UpShop.Dominio.Interfaces;

namespace UpShop.Dominio.Entitys
{
    public class Entity : IEntity
    {
        public ObjectId Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCriation { get; set; }
        public DateTime? DateModification { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }

        public Entity()
        {
            IsActive = true;
        }
    }

}
