namespace DATA.DTO
{
    public class LoginResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public object Token { get; set; }

        public UserRsponseDto UserRsponse { get; set; }

        public string ErrorDetails { get; set; }


    }
}