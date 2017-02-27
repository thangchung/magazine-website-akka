using System;

namespace Cik.Magazine.Shared
{
    public static class SystemClock
    {
        private static Func<DateTime> _getUtcNow = () => DateTime.UtcNow;
        private static Func<DateTimeOffset> _getOffsetUtcNow = () => DateTimeOffset.UtcNow;

        public static DateTime UtcNow => _getUtcNow();

        public static DateTimeOffset OffsetUtcNow => _getOffsetUtcNow();

        public static IDisposable Set(DateTime dateTime)
        {
            _getUtcNow = () => dateTime;
            return new DisposableStub();
        }

        public static IDisposable Set(DateTimeOffset dateTimeOffset)
        {
            _getOffsetUtcNow = () => dateTimeOffset;
            return new DisposableStub();
        }

        private class DisposableStub : IDisposable
        {
            public void Dispose()
            {
                _getUtcNow = () => DateTime.UtcNow;
                _getOffsetUtcNow = () => DateTimeOffset.UtcNow;
            }
        }
    }
}