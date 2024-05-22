using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, Data = data, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {   
            return new Response<T> { StatusCode = statusCode, Data = default, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = new List<string>
                {
                    errors
                },
                IsSuccessful = false,
                StatusCode = statusCode
            };
        }
    }
}
