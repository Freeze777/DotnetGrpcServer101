using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService101.Mapper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GrpcService101.Services
{
    public class BlogService : BlogApp.BlogAppBase
    {
        private static readonly MongoClient Client = new("mongodb://localhost:27017");
        private static readonly IMongoDatabase Db = Client.GetDatabase("blogdb");
        private static readonly IMongoCollection<BsonDocument> Blogs = Db.GetCollection<BsonDocument>("blog");


        public override Task<Blog> Create(CreateBlogRequest request, ServerCallContext context)
        {
            var doc = new BsonDocument()
                .Add("author", request.Author)
                .Add("title", request.Title)
                .Add("content", request.Content);
            Blogs.InsertOne(doc);
            var blog = new Blog
            {
                Id = doc.GetValue("_id").ToString(),
                Author = request.Author,
                Content = request.Content,
                Title = request.Title
            };
            return Task.FromResult(blog);
        }

        public override async Task<Blog> GetBlog(GetBlogRequest request, ServerCallContext context)
        {
            var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(request.Id));
            var result = (await Blogs.FindAsync(filter)).FirstOrDefault();
            if (result is null)
                throw new RpcException(new Status(StatusCode.NotFound, "Blog not found"));
            return BlogMapper.Map(result);
        }

        public override async Task<ListBlogResponse> ListBlog(Empty request, ServerCallContext context)
        {
            var blogs = (await Blogs.FindAsync(_ => true)).ToList().Select(BlogMapper.Map);
            var response = new ListBlogResponse();
            response.Blogs.AddRange(blogs);
            return response;
        }
    }
}
