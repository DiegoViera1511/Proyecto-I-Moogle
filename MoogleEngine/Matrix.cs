namespace MoogleEngine
{
    static class Matrix
    {
        public static int[,] Transpuesta(int[,] A)
        {
            int[,] result = new int[A.GetLength(1),A.GetLength(0)];
            for(int i = 0 ; i < A.GetLength(0) ; i++)
            {
                for(int j = 0 ; j < A.GetLength(1) ; j++)
                {
                    result[j,i] = A[i,j];
                }
            }
            return result;
        }
        public static void PrintMatrix(int[,] x)
        {
            for(int i = 0 ; i < x.GetLength(0) ; i++){
                Console.Write("| "+x[i,0]+" ");
                for(int j = 1 ; j < x.GetLength(1) ; j++){
                    if(j == x.GetLength(1) -1){
                        Console.Write(x[i,j] + " |");
                    }else  Console.Write(x[i,j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static int[,] Suma(int[,] A , int[,] B)
        {
            int f = A.GetLength(0);
            int c = A.GetLength(1);
            int[,] result = new int[f,c];
            if(B.GetLength(0) != f || B.GetLength(1) != c)
            {
                throw new ArgumentException("Las matrices deben pertenecer al mismo orden");
            }
            for(int i = 0 ; i < f ; i++)
            {
                for(int j = 0 ; j < c ; j++)
                {
                    result[i,j] = A[i,j] + B[i,j];
                }
            }
            return result;
        }
        public static int[,] Product(int[,] A , int[,] B)
        {
            int f = A.GetLength(0);
            int c = A.GetLength(1);
            int[,] result = new int[f , B.GetLength(1)];

            if(c != B.GetLength(0))
            {
                throw new ArgumentException("La dimensión de las columnas de A deben ser igual a la dimensión de las filas de B");
            }

            for(int i = 0 ; i < f ; i++)//Iterando por las filas de A
            {
                for(int j = 0 ; j < B.GetLength(1) ; j++)//Iterando por las columnas de B
                {
                   int Temp = 0;

                   for(int k = 0 ; k < c ; k++)//Iterando por las columnas de A
                   {
                        Temp += A[i,k] * B[k,j]; 
                   }
                   result[i,j] = Temp; 
                }
            }
            return result;
        }
        public static int[,] MultEscalar(int escalar ,int[,] A)
        {
            int[,] result = new int[A.GetLength(0),A.GetLength(1)];
            for(int i = 0 ; i < A.GetLength(0) ; i++)
            {
                for(int j = 0 ; j < A.GetLength(1) ; j++)
                {
                    result[i,j] = escalar * A[i,j];
                }
            }
            return result ;
        }

        public static double Determinante(double[,] A)
        {
            if(A.GetLength(0) != A.GetLength(1))
            {
                throw new Exception("La matriz debe ser cuadrada");
            }
            return Determinante2(A , A.GetLength(0));    
        }
        private static double Determinante2(double[,] A , int N)
        {
            double det = 0 ;
            if(N == 2)
            {
                return A[0,0] * A[1,1] - (A[1,0] * A[0,1]);
            }
           
            for(int i = 0 ; i < A.GetLength(0) ; i++)
            {
                det += A[0,i] * Math.Pow(-1 , i) * Determinante(Menor(A , 0 , i));
            }
            return det;
        }

        static double[,] Menor(double[,] A , int i , int j) //A tiene que ser cuadrada 
        {
           int rg = A.GetLength(0); 
           List<double> values = new List<double>();

            for(int a = 0 ; a < A.GetLength(0) ; a++)
            {
                if(a == i)
                {
                    continue;
                }
                for(int b = 0 ; b < A.GetLength(1) ; b++)
                {
                    if(b == j)
                    {
                        continue;
                    }
                    values.Add(A[a,b]);
                }
            }
            double[,] result = new double[rg -1 , rg - 1];
            int count = 0;
            for(int c = 0 ; c < rg - 1 ; c++)
            {
                for(int d = 0 ; d < rg -1 ; d++)
                {
                    result[c,d] = values[count];
                    count++;
                }
            }
            return result;
        }
    }  
}