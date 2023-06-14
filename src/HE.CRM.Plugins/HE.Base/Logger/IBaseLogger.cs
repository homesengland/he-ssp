namespace HE.Base.Log
{
    public interface IBaseLogger
    {
        LogLevel LogLevel { get; }

        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);
    }
}
