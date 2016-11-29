using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BFFP
{
    public abstract class Parser
    {
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
            throw new NotImplementedException();
        }
    }
}
