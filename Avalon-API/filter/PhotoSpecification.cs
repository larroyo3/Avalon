using Avalon_API.Models;

public class PhotoSpecification : BaseSpecification<PhotoItem>
{
    public PhotoSpecification(int id)
        : base(b => b.AuthorId == id)
    {
        AddInclude(b => b);
    }
    public PhotoSpecification(string item)
        : base(b => b.Hashtags == item)
    {
        AddInclude(b => b);
    }
}