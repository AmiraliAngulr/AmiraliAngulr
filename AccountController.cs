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
    public class AccountController : ControllerBase
    {
        #region cunstructor

        private readonly IMongoCollection<AppUser> _collection;

        // Dependency Injection
        public AccountController(IMongoClient client, IMongoDbSettings dbSettings)
        {
            var dbName = client.GetDatabase(dbSettings.DatabaseName);
            _collection = dbName.GetCollection<AppUser>("players");
        }

        #endregion

        [HttpPost("login")]
        public ActionResult<AppUser> CreateUser(AppUser userInput)
        {
                AppUser user = new AppUser(
                    Id: null,
                    IdUser: userInput.IdUser,
                    IdBook: userInput.IdBook,
                    Name: userInput.Name,
                    Itme: userInput.Itme,
                    Price: userInput.Price
                );
                
                _collection.InsertOne(user);
        
                return  Ok(user);
        }
        
        [HttpGet("getAll")]
        public ActionResult<List<AppUser>> GetAll()
        {
            List<AppUser> appusers = _collection.Find<AppUser>(new BsonDocument()).ToList();
            
            if(appusers.Count == 0)
                return NotFound("No users found");
            
                return appusers;
        }
        
        [HttpGet("get-by-id/{id}")]
        public ActionResult<AppUser> GetUserById(string id)
        {
            AppUser user = _collection.Find(u => u.Name == id).FirstOrDefault();
            if (user is null)
            {
                return NotFound("User does not exist");
            }
            
            return user;
        }
        
        [HttpPut("update-user/{userId}")]
        public ActionResult<UpdateResult> UpdateUser(string userId, AppUser user)
        {
            UpdateDefinition<AppUser> updateName = Builders<AppUser>.Update
                .Set(u => u.Name, user.Name.Trim().ToLower());
                
                return _collection.UpdateOne(u => u.Id == userId, updateName);
        }
        
        [HttpDelete("delete-user/{userId}")]
        public ActionResult<DeleteResult> DeleteUser(string userId)
        {
          AppUser user =_collection.Find(doc => doc.Id == userId).FirstOrDefault();
        
          if (user is null)
           return NotFound("User does not exist");
          
          return _collection.DeleteOne(doc => doc.Id == userId);
        }
        
    }
}
