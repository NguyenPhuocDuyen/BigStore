using Microsoft.AspNetCore.Http;

namespace BigStore.Utility.Validation
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
