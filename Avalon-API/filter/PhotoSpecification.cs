using Avalon_API.Models;

public class PhotoSpecification : BaseSpecification<PhotoItem>
{
    public PhotoSpecification(int basketId)
        : base(b => b.AuthorId == basketId)
    {
        AddInclude(b => b);
    }
    public PhotoSpecification(string buyerId)
        : base(b => b.Hashtags == buyerId)
    {
        AddInclude(b => b);
    }
}