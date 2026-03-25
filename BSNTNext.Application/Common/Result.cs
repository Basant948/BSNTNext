using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Common
{
    public class Result
    {
        public bool Succeeded { get; set; }

        public string? Message { get; set; }

        public List<string> Errors { get; set; } = new();

        public static Result Success(string? message = null)
        {
            return new Result
            {
                Succeeded = true,
                Message = message ?? ""
            };
        }

        public static Result Failure(string? message = null)
        {
            return new Result
            {
                Succeeded = false,
                Message = message ?? "" 
            };
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result
            {
                Succeeded = false,
                Errors = errors.ToList()
            };
        }
    }
}
