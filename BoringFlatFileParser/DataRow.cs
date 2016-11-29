using System.Collections.Generic;

namespace BFFP
{
    public struct DataRow
    {
        public List<Field> Fields;
        public char[] Buffer;
        public IReadOnlyDictionary<string, int> NameLookup;

        public DataRow(List<Field> fields, char[] buffer, IReadOnlyDictionary<string, int> nameLookup)
        {
            this.Fields = fields;
            this.Buffer = buffer;
            this.NameLookup = nameLookup;
        }

        public DataRow(DataRow dataRow)
        {
            this.Fields = dataRow.Fields;
            this.Buffer = dataRow.Buffer;
            this.NameLookup = dataRow.NameLookup;
        }
    }
}