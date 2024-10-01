namespace TaskingSystem.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public static ApiResponse<T> SuccessResponse(T data) => new() { Success = true, Data = data };
        public static ApiResponse<T> ErrorResponse(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
