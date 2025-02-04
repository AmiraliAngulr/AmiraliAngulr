using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region cunstructor

        private readonly IMongoCollection<LoginUser> _collection;

        // Dependency Injection
        public UserController(IMongoClient client, IMongoDbSettings dbSettings)
        {
            var dbName = client.GetDatabase(dbSettings.DatabaseName);
            _collection = dbName.GetCollection<LoginUser>("loginUser");
        }

        #endregion

        [HttpPost("login")]
        public ActionResult<LoginUser> CreateUser(LoginUser userInput)
        {
            LoginUser login = _collection.Find(u => u.Email == userInput.Email).FirstOrDefault();
            if (login == null)
            {
                LoginUser user = new LoginUser(
                    Id: null,
                    Name: userInput.Name.Trim().ToLower(),
                    Email: userInput.Email.Trim().ToLower(),
                    Password: userInput.Password,
                    Age: userInput.Age,
                    Address: new Address(
                             Phone: userInput.Address.Phone,
                            Country: userInput.Address.Country,
                            PostCode: userInput.Address.PostCode,
                            StreetNumber: userInput.Address.StreetNumber,
                            Street: userInput.Address.Street,
                            City: userInput.Address.City,
                            ZipCod: userInput.Address.ZipCod
                            )
                );

                _collection.InsertOne(user);

                return user;
            }
            else
            {
                return NotFound("Email address already exists");
            }
        }

        [HttpGet("getAll")]
        public ActionResult<List<LoginUser>> GetAll()
        {
            List<LoginUser> loginuser = _collection.Find<LoginUser>(new BsonDocument()).ToList();

            if (loginuser.Count == 0)
                return NotFound("No users found");

            return loginuser;
        }

        [HttpGet("get-by-id/{id}")]
        public ActionResult<LoginUser> GetUserById(string id)
        {
            LoginUser user = _collection.Find(u => u.Name == id).FirstOrDefault();
            if (user is null)
            {
                return NotFound("User does not exist");
            }

            return user;
        }

        [HttpPut("update-user/{userId}")]
        public ActionResult<UpdateResult> UpdateUser(string userId, LoginUser user)
        {
            UpdateDefinition<LoginUser> updateEmail = Builders<LoginUser>.Update
                .Set(u => u.Email, user.Email.Trim().ToLower());
            UpdateDefinition<LoginUser> updateName = Builders<LoginUser>.Update
                .Set(u => u.Name, user.Name.Trim().ToLower());

            return _collection.UpdateOne(u => u.Id == userId, updateEmail);
            return _collection.UpdateOne(u => u.Id == userId, updateName);
        }

        [HttpDelete("delete-user/{userId}")]
        public ActionResult<DeleteResult> DeleteUser(string userId)
        {
            LoginUser user = _collection.Find(doc => doc.Id == userId).FirstOrDefault();

            if (user is null)
                return NotFound("User does not exist");

            return _collection.DeleteOne(doc => doc.Id == userId);
        }

    }
}
