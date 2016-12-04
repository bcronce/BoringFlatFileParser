using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BFFP
{
    public class FixedWidth : Parser
    {
        public struct Field
        {
            public int Length;

            public Field(int length)
            {
                this.Length = length;
            }
        }

        protected Field[] Fields;

        protected override Task<bool> InternalRead(DataRow reuse, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public FixedWidth(StreamReader input, List<Field> fields)
            : base(input, true)
        {
            this.Fields = fields.ToArray();
        }
    }
}
