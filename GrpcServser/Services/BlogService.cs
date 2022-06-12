using System.Threading.Tasks;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GrpcService101.Services
{
    public class BlogService : BlogApp.BlogAppBase
    {
        private static readonly MongoClient Client = new MongoClient("mongodb://localhost:27017");
        private static readonly IMongoDatabase Db = Client.GetDatabase("blogdb");
        private static readonly IMongoCollection<BsonDocument> blogs = Db.GetCollection<BsonDocument>("blog");


        public override Task<Blog> Create(Blog request, ServerCallContext context)
        {
            var doc = new BsonDocument().Add("author", request.Author).Add("title", request.Title);
            blogs.InsertOne(doc);
            request.Id = doc.GetValue("_id").ToString();
            return Task.FromResult(request);
        }
    }
}
