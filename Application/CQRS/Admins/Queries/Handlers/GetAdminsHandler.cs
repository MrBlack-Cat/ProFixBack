using Application.CQRS.Admins.Queries.Requests;
using Application.CQRS.Users.DTOs;
using AutoMapper;
using MediatR;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Admins.Queries.Handlers
{
    public class GetAdminsHandler : IRequestHandler<GetAdminsQuery, IEnumerable<UserListDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAdminsHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserListDto>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
        {
            var admins = await _userRepository.GetUsersByRoleAsync(1); // RoleId = 1 Admin
            return _mapper.Map<IEnumerable<UserListDto>>(admins);
        }
    }

}
