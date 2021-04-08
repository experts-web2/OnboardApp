using System;

namespace DTOs.ResponseDtos
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Token { get; set; }       
    }
}