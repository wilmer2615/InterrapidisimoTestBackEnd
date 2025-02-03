using DataTransferObjects;

namespace Business.LoginLogic
{
    public interface ILoginLogic
    {
        public Task<StudentDto> AddAsync(StudentDto studenDto);
        public Task<StudentDto?> VerifyAccountAsync(AccountDto accountDto);
    }
}
