using System;

namespace RLTestTask.Exceptions
{
    public class UserServiceException : Exception
    {
        private string _message;
        public override string Message { get => _message; }
        public UserServiceException(string message) : base(message) 
        {
            this._message = message;
        }
    }
}