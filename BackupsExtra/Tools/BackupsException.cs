﻿using System;

namespace BackupsExtra.Tools
{
    public class BackupsException : Exception
    {
        public BackupsException()
            {
            }

        public BackupsException(string message)
                : base(message)
            {
            }

        public BackupsException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }
}