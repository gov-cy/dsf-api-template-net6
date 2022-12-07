namespace dsf_api_template_net6.Models
{
    //public class BaseResponse
    //{
    //    public int ErrorCode { get; set; }
    //    public string? ErrorMessage { get; set; }
    //    public bool Succeeded { get { return ErrorCode == 0; } }
    //}

    public class BaseResponse<T>
    {
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public bool Succeeded { get { return ErrorCode == 0; } }
        public string? InformationMessage { get; set; }
    }
}
