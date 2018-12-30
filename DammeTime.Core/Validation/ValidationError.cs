using System;
using System.Collections.Generic;

namespace DammeTime.Core.Validation
{
    public class ValidationError : Exception
    {
        public ValidationError() : base() { Errors = new List<Error>(); }
        public ValidationError(string message) : base(message) { Errors = new List<Error>(); }
        public ValidationError(string message, Exception ex) : base(message, ex) { Errors = new List<Error>(); }

        public ValidationError(params Error[] errors) : base() { Errors = errors; }
        public ValidationError(string message, params Error[] errors) : base(message) { Errors = errors; }
        public ValidationError(string message, Exception ex, params Error[] errors) : base(message, ex) { Errors = errors; }

        public IEnumerable<Error> Errors { get; set; }

        public class Error
        {
            public string Property { get; set; }
            public string Message { get; set; }

            public Error(string property, string message)
            {
                Property = property;
                Message = message;
            }
        }
    }
}
