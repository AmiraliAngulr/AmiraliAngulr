using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Model;

public record BookUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    string? Id,
    string NameBook, //اسم کتاب
    string DescriptionBook, //ژانر کتاب
    int AgeBook, //سال عرضه کتاب
    bool InventoryBook //موجودی کناب
    );