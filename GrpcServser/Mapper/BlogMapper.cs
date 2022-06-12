using MongoDB.Bson;

namespace GrpcService101.Mapper
{
    public static class BlogMapper
    {
        public static Blog Map(BsonDocument document)
        {
            return new Blog
            {
                Id = document.GetValue("_id").ToString(),
                Title = document.GetValue("title").ToString(),
                Author = document.GetValue("author").ToString(),
                Content = document.GetValue("content").ToString()
            };
        }
    }
}
