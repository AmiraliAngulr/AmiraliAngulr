using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record LoginUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    string? Id, // hamishe sabet
    [MinLength(3)] [MaxLength(30)] string Name,
    [EmailAddress] string Email,
    [MinLength(8)] [MaxLength(16)] string Password,
    [Range(7, 99)] int Age,
    Address Address
    );

    public record Address(
    [MaxLength(22)] [MinLength(11)] int ZipCod,
        string City,
        string Street,
        string StreetNumber,
        [MaxLength(42)] [MinLength(21)]  int PostCode,
        string Country,
       [MaxLength(11)] int Phone
        );