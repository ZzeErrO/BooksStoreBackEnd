using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class BookStoreCustomExceptions : Exception
    {
        public enum ExceptionType
        {
            USER_EXIST,
            INVALID_EMAIL
        }

        private readonly ExceptionType type;
        public BookStoreCustomExceptions(ExceptionType Type, String message) : base(message)
        {
            this.type = Type;
        }
    }
}
