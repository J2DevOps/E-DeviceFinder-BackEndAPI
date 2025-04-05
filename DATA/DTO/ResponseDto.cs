namespace DATA.DTO
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }

        public string ErrorDetails { get; set; }
    }
}
