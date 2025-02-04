using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models;

public record AppUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    string? Id, // hamishe hast
    string IdUser,
    string IdBook,
    [MinLength(3)] [MaxLength(30)] string Name,
    string Itme,
    double Price
);