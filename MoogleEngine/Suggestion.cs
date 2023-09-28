using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class SuggestionClass
    {
        public static int LevenshteinDistance(string s , string t)
        {
            int result = 0;
            int costo = 0;
            int f = s.Length; //Filas 
            int c = t.Length; //Columnas
            int[,] m = new int[ f + 1 , c + 1 ];

            //Relleno la primera fila y columna con el tamaño de cada palabra respectivamente
            for(int i = 1 ; i <= f ; m[i , 0] = i++) ;
            for(int j = 1 ; j <= c ; m[0 , j] = j++) ;

            //Recorro la matriz llenando cada uno de los pesos
            // i filas , j columnas
            for(int i = 1 ; i <= f ; i++)
            {
                for(int j = 1 ; j <= c ; j++)
                {
                    //Si los caracteres equidistantes son diferentes el costo es 1 sino es 0
                    costo = (s[i-1] == t[j-1]) ? 0 : 1 ; 
                    m[ i , j ] = Math.Min(Math.Min(m[i-1 , j] + 1 , m[i , j-1] + 1) , m[i - 1 , j - 1] + costo) ;
                }
            }

            result = m[ f , c ];

            return result;
        }

        public static string Suggestion(Dictionary<string,double> vectorquery)
        {
            string result = "";
            List<string> resultwords = new List<string>();
            bool makeSugg = false;
            foreach(string s in vectorquery.Keys)
            {
                if(!TFxIDF.TodasLasPalabras.ContainsKey(s))
                {
                    makeSugg = true;
                }
            }

            if(makeSugg)
            {
                foreach(string s in vectorquery.Keys)
                {
                    int cost = int.MaxValue;
                    string SuggWord = "";

                    if(TFxIDF.TodasLasPalabras.ContainsKey(s))
                    {
                        resultwords.Add(s);
                    }
                    else
                    {
                        foreach(string t in TFxIDF.TodasLasPalabras.Keys)
                        {
                            int tempCost = 0;
                            tempCost = LevenshteinDistance(s , t);
                            if(tempCost < cost)
                            {
                                SuggWord = t;
                                cost = tempCost;
                            }
                        }
                        resultwords.Add(SuggWord);
                    }
                }
            }
            else return result;    
            
            for(int i = 0 ; i < resultwords.Count ; i++)
            {
                if(i == resultwords.Count - 1) 
                {
                    result += resultwords[i];//Mi última palabra no nececita el espacio final
                    break;
                }
                result += resultwords[i] + " ";
            }

            return result;
        }
    }
}