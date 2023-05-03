using Microsoft.AspNetCore.Authorization;

namespace BigStore.Security.Requirements
{
    public class GenZRequirement : IAuthorizationRequirement
    {
        public int MinYear { get; }

        public int MaxYear { get; }

        public GenZRequirement(int _minYear = 1950, int _maxYear = 2023)
        {
            MinYear = _minYear;
            MaxYear = _maxYear;
        }
    }
}
