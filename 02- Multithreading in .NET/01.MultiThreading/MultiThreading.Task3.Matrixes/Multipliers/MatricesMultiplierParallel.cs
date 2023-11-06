﻿using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;
using System;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            if (m1.ColCount != m2.RowCount)
                throw new InvalidOperationException("Matrix dimensions are not compatible for multiplication.");

            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, m1.RowCount, i =>
            {
                for (byte j = 0; j < m2.ColCount; j++)
                {
                    long sum = 0;
                    for (byte k = 0; k < m1.ColCount; k++)
                    {
                        sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                    }
                    resultMatrix.SetElement(i, j, sum);
                }
            });

            return resultMatrix;
        }
    }
}
