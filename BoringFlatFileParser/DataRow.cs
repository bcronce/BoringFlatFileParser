using System.Collections.Generic;

namespace BFFP
{
    public struct DataRow
    {
        public List<Field> Fields;
        public char[] Buffer;

        public DataRow(List<Field> fields, char[] buffer)
        {
            this.Fields = fields;
            this.Buffer = buffer;
        }

        public DataRow(DataRow dataRow)
        {
            this.Fields = dataRow.Fields;
            this.Buffer = dataRow.Buffer;
        }
    }
}