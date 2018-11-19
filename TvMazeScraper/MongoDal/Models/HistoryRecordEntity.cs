using System;
using MongoDB.Bson;

namespace MongoDal.Models
{
    public class HistoryRecordEntity
    {
        public ObjectId Id { get; set; }

        public DateTime UpdateProcessStartDate { get; set; }
    }
}
