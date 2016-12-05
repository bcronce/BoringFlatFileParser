using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace BFFP
{
    public abstract class Parser : IDisposable
    {
        public enum Trim
        {
            NoTrim,
            LeftTrim,
            RightTrim,
            LeftAndRightTrim
        }

        private Trim TrimType;
        private bool LeaveOpen;
        protected StreamReader Stream;
        private bool ClearRowBuffer;

        private DataRow SetRecord;
        private bool IsRecordSet = false;
        protected DataRow Record {
            set
            {
                if (value.Buffer == null)
                    throw new InvalidOperationException($"{nameof(value.Buffer)} may not be null");

                if (value.Fields == null)
                    throw new InvalidOperationException($"{nameof(value.Fields)} may not be null");

                IsRecordSet = true;

                this.SetRecord = value;
            }
            private get
            {
                //Throw a tantrum if the datarow is not set
                if (!IsRecordSet)
                    throw new InvalidOperationException($"DataRow cannot be acquired until it has been read");

                return SetRecord;
            }
        }

        #region Public Methods
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

            reuse.Fields.Clear();

            if (this.ClearRowBuffer)
                Array.Clear(reuse.Buffer, 0, reuse.Buffer.Length);

            return this.InternalRead(reuse, cancellationToken);
        }
        #endregion

        #region Protected Methods
        protected abstract Task<bool> InternalRead(DataRow reuse, CancellationToken cancellationToken);
        #endregion

        public DataRow GetRecord()
        {
            return this.Record;
        }

        public Parser(StreamReader input)
            : this(input, Trim.NoTrim, false)
        { }

        public Parser(StreamReader input, Trim trimType)
            : this(input, trimType, false)
        { }

        public Parser(StreamReader input, Trim trimType, bool clearRowBufferOnRead)
            : this(input, trimType, clearRowBufferOnRead, true)
        { }

        protected Parser(StreamReader input, Trim trimType, bool clearRowBufferOnRead, bool leaveOpen)
        {
            this.LeaveOpen = leaveOpen;
            this.Stream = input;
            this.TrimType = trimType;
            this.ClearRowBuffer = clearRowBufferOnRead;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                // Note that Stream.Close() can potentially throw here. So we need to
                // ensure cleaning up internal resources, inside the finally block.
                if (!LeaveOpen && disposing && (this.Stream != null))
                {
                    this.Stream.Dispose();
                }
            }
            finally
            {
                if (!LeaveOpen && (this.Stream != null))
                {
                    this.Stream = null;
                }
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
