﻿namespace bookmarkify
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Warn(string message);
    }
}