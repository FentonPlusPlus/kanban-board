using System;
using System.Collections.Generic;
using System.Linq;
using ProjectKanban.Controllers;

namespace ProjectKanban.Users
{
    public sealed class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AllUsersResponse GetAllUsers()
        {
            var userRecords = _userRepository.GetAll();
            var response = new AllUsersResponse {Users = new List<UserModel>()};

            foreach (var userRecord in userRecords)
            {
                response.Users.Add(new UserModel
                {
                    Username = userRecord.Username
                });
            }

            return response;
        }

        public Session Login(LoginRequest loginRequest)
        {
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
            if (user != null)
                return new Session
                {
                    Username = user.Username,
                    UserId = user.Id,
                    ClientId = user.ClientId
                };
            throw new Exception("Invalid credentials");
        }
    }
}