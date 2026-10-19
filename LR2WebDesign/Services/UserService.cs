using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Exceptions;
using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace LR2WebDesign.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> CreateUser(UserCreateRequestDTO UserCreateRequest, IConfiguration config)
        {
            User user = new User
            {
                Login = UserCreateRequest.Login,
                Name = UserCreateRequest.Name,
                Password = UserCreateRequest.Password,
                Email = UserCreateRequest.Email,
            };

            var userExistsObj = await _userRepository.GetUser(user);

            if (userExistsObj.Item1)
            {
                throw new UserAlreadyExistsException(userExistsObj.Item2);
            }
            else
            { 
                await _userRepository.Create(user);
                var tokenString = GenerateJwtTokenString(user, config);
                return tokenString;
            }
            
        }

        public async Task<string> LoginUser(UserLoginRequestDTO userLoginRequest, IConfiguration config)
        {
            Regex emailRegex = new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);
            User user = null;
            if (emailRegex.IsMatch(userLoginRequest.LoginOrEmail))
            {
                user = new User
                {
                    Login = null,
                    Name = null,
                    Password = userLoginRequest.Password,
                    Email = userLoginRequest.LoginOrEmail,
                };
            }
            else
            {
                user = new User
                {
                    Login = userLoginRequest.LoginOrEmail,
                    Name = null,
                    Password = userLoginRequest.Password,
                    Email = null
                };
            }
            

            var userExistsObj = await _userRepository.GetUser(user);

            if (!userExistsObj.Item1)
            {
                throw new GetUserException("Пользователь не найден.");
            }

            
            if (userExistsObj.Item3.Password != userLoginRequest.Password)
            {
                throw new InvalidCredentialsException();
            }
            else
            {
                var tokenString = GenerateJwtTokenString(userExistsObj.Item3, config);
                return tokenString;
            }
            
        }


        private string GenerateJwtTokenString(User userExistsObj, IConfiguration config)
        {
            var claims = new List<Claim>
            {
                new Claim("Login", userExistsObj.Login),
                new Claim("Email", userExistsObj.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                        issuer: config["Jwt:Issuer"],
                        audience: config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(15),
                        signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
