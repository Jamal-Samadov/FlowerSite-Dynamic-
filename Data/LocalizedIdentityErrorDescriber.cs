using Microsoft.AspNetCore.Identity;

namespace FlowerSite.Data
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Email təkrarlana bilməz"
            };
        }
    }
}
