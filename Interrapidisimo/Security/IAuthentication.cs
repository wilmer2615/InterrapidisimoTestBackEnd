using DataTransferObjects;

namespace Interrapidisimo.Security
{
    public interface IAuthentication
    {
        public string GenerateToken(StudentDto studentDto);
    }
}
