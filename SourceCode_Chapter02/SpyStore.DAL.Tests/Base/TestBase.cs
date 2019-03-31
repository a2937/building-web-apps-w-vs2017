using SpyStore.DAL.EF;
using System;

namespace SpyStore.DAL.Tests.Base
{
    public abstract class TestBase : IDisposable
    {
        protected readonly StoreContext _db;

        private bool disposedValue = false; // To detect redundant calls

        protected TestBase()
        {
            _db = new StoreContext();
            CleanDatabase();
        }

        protected abstract void CleanDatabase();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CleanDatabase();
                    _db.Dispose();
                }

                disposedValue = true;
            }
        }

        ~TestBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}