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
    }  
}