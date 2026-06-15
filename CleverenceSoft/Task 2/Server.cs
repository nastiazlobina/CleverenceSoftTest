
public static class Server
{
    private static int _count;
    private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

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