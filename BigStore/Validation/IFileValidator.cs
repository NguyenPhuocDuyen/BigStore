namespace BigStore.Validation
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
