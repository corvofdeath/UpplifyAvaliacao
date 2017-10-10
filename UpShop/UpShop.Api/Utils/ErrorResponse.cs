namespace UpShop.Api.Utils
{
    public class ErrorResponse
    {
        public string Error { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(string message)
        {
            Error = message;
        }
    }
}
