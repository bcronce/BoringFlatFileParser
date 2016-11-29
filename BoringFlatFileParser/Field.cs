namespace BFFP
{
    public struct Field
    {
        public int Offset;
        public int Length;

        public Field(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }

        public Field(Field field)
        {
            this.Offset = field.Offset;
            this.Length = field.Length;
        }
    }
}