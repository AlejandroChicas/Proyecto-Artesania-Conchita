using System;

namespace Prototipo
{
    internal class TuDbContext : IDisposable
    {
        public object Clientes { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}