﻿using System;

namespace SHInspect.Classes
{
    public class TestCrashException : Exception
    {
        public TestCrashException()
        {
        }

        public TestCrashException(string message)
            : base(message)
        {
        }

        public TestCrashException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
