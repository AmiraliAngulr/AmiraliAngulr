using api.Model;
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
    public class BookController : ControllerBase
    {
        #region cunstructor

        private readonly IMongoCollection<BookUser> _collection;

        // Dependency Injection
        public BookController(IMongoClient client, IMongoDbSettings dbSettings)
        {
            var dbName = client.GetDatabase(dbSettings.DatabaseName);
            _collection = dbName.GetCollection<BookUser>("namebook");
        }

        #endregion

        [HttpPost("create")]
        public ActionResult<BookUser> Create(BookUser bookUser)
        {
            BookUser book = _collection.Find(b => b.NameBook == bookUser.NameBook).FirstOrDefault();
            if (book == null)
            {
                BookUser user = new BookUser(
                    Id: null,
                    NameBook: bookUser.NameBook.Trim().ToLower(),
                    DescriptionBook: bookUser.DescriptionBook,
                    AgeBook: bookUser.AgeBook,
                    InventoryBook: bookUser.InventoryBook
                );
                _collection.InsertOne(user);
                return user;
            }

            else
            {
                return NotFound("Mester with this name book already exists");
            }
        }
        [HttpGet("getAll")]
        public ActionResult<List<BookUser>> GetAll()
        {
            List<BookUser> bookUsers = _collection.Find<BookUser>(new BsonDocument()).ToList();
            
            if (bookUsers.Count == 0)
                return NotFound("Mester with this name book already exists");
            
            return bookUsers;
        }

        [HttpGet("get/{id}")]
        public ActionResult<BookUser> Get(string id)
        {
            BookUser Book = _collection.Find(b => b.NameBook.Trim().ToLower() == id).FirstOrDefault();
            if (Book is null)
            {
                return NotFound("Mester with this name book already exists");
            }
            
            return Book;
        }

        [HttpPut("update/{id}")]
        public ActionResult<UpdateResult> Update(string id, BookUser bookUser)
        {
            UpdateDefinition<BookUser> bookUserUpdate = Builders<BookUser>.Update
                .Set(bookUser => bookUser.NameBook, bookUser.NameBook);
            
            return _collection.UpdateOne(u => u.Id == id, bookUserUpdate);
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<DeleteResult> Delete(string id)
        {
           BookUser bookUser = _collection.Find(b => b. NameBook.Trim().ToLower() == id).FirstOrDefault();

           if (bookUser is null)
            return NotFound("Mester with this name book already exists");
           
           return _collection.DeleteOne(b => b.NameBook == id);
        }
    }
}
