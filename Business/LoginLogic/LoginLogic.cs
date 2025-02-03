using AutoMapper;
using DataTransferObjects;
using Entities;
using Repository.Repository.LoginRepository;

namespace Business.LoginLogic
{
    public class LoginLogic : ILoginLogic
    {

        private readonly IMapper _mapper;

        private readonly ILoginRepository _loginRepository;


        public LoginLogic(IMapper mapper, ILoginRepository loginRepository)
        {
            this._mapper = mapper;
            this._loginRepository = loginRepository;
        }
        public async Task<StudentDto> AddAsync(StudentDto studentDto)
        {
            var entity = await this._loginRepository.AddAsync(_mapper.Map<Student>(studentDto));

            var result = _mapper.Map<StudentDto>(entity);

            return result;
        }

        public async Task<StudentDto?> VerifyAccountAsync(AccountDto accountDto)
        {
            var entity = await this._loginRepository.VerifyAccountAsync(accountDto.Email, accountDto.Password);

            var result = _mapper.Map<StudentDto>(entity);

            return result;
        }
    }
}
