using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BFFP
{
    public abstract class Parser : IDisposable
    {
        public static string GetStringByOrdinal(DataRow record, int ordinal)
        {
            var field = record.Fields[ordinal];

            if (field.Length == 0)
                return string.Empty;
            else
                return new string(record.Buffer, field.Offset, field.Length);
        }

        public static string GetStringByName(DataRow record, string name)
        {
            int fieldOrdinal;

            if (record.NameLookup.TryGetValue(name, out fieldOrdinal))
                return GetStringByOrdinal(record, fieldOrdinal);

            throw new KeyNotFoundException($"'{name}' is not a valid field name");
        }

        public Task<bool> Read(DataRow reuse, CancellationToken cancellationToken)
        {
            if (reuse.Buffer == null)
                throw new ArgumentException($"{nameof(reuse.Buffer)} may not be null", nameof(reuse));

            if (reuse.Fields == null)
                throw new ArgumentException($"{nameof(reuse.Fields)} may not be null", nameof(reuse));

            return this.InternalRead(reuse, cancellationToken);
        }

        protected abstract Task<bool> InternalRead(DataRow reuse, CancellationToken cancellationToken);

        public DataRow GetRecord()
        {
            var tmpRecord = this.Record;

            //Throw a tantrum if the record is not set
            if (tmpRecord.Fields == null)
                throw new InvalidOperationException($"Cannot call {nameof(GetRecord)} without {nameof(Read)} returning true");

            //Reset once consumed
            this.Record = new DataRow(null, null, null);

            return tmpRecord;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Parser() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
