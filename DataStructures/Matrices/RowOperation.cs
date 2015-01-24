namespace DataStructures.Matrices
{
    public class RowOperation
    {
        public static RowOperation Swap(Matrix m, int rowIndex1, int rowIndex2)
        {
            Vector<double> r1Copy = m.GetRow(rowIndex1);
            for (int j = 0; j < m.NumColumns; j++)
            {
                m[rowIndex1, j] = m[rowIndex2, j];
                m[rowIndex2, j] = r1Copy[j];
            }
            return new RowOperation(rowIndex1, rowIndex2);
        }

        public static RowOperation Multiply(Matrix m, int rowIndex, double value)
        {
            for (int j = 0; j < m.NumColumns; j++)
            {
                m[rowIndex, j] *= value;
            }
            return new RowOperation(rowIndex, rowIndex, value);
        }

        public static RowOperation AddRow(Matrix m, int rowIndex, int rowToAddIx, double multiplier = 1)
        {
            for (int j = 0; j < m.NumColumns; j++)
            {
                m[rowIndex, j] += (multiplier * m[rowToAddIx, j]);
            }
            return new RowOperation(rowIndex, rowToAddIx, multiplier);
        }

        private RowOperation(int rowIndex, int rowAdded, double multiplier = 1)
        {
            RowIndex = rowIndex;
            RowAdded = rowAdded;
            Multiplier = multiplier;
        }

        public int RowIndex { get; set; }
        public int RowAdded { get; set; }
        public double Multiplier { get; set; }
    }
}