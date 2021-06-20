namespace WeatherApp.Helper
{
    public class BaseResponse
    {
        public int ErrorCode { get; set; } = 0;
        public string Description { get; set; } = "Success";
    }
}
