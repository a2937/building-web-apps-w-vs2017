using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.DAL.Tests.Base
{
    public abstract class TestBase : IDisposable
    {
        protected readonly StoreContext _db;

        protected TestBase()
        {
            _db = new StoreContext();
            CleanDatabase();
        }


        protected abstract void CleanDatabase();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    //
                }
                CleanDatabase();
                _db.Dispose();
                disposedValue = true;
            }
        }

         ~TestBase()
        {

          Dispose(false);
         }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
