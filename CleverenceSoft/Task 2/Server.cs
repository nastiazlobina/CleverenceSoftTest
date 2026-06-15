namespace CleverenceSoft.Task_1;


public static class Server
{
    private static int _count;
    private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    /// <summary>
    /// Получает текущее значение счётчика
    /// </summary>
    public static int GetCount()
    {
        _lock.EnterReadLock();
        try
        {
            return _count;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Увеличивает или уменьшает счётчик на значение
    /// </summary>
    public static void AddToCount(int value)
    {
        _lock.EnterWriteLock();
        try
        {
            _count += value;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Сброс счётчика
    /// </summary>
    public static void Reset()
    {
        _lock.EnterWriteLock();
        try
        {
            _count = 0;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}