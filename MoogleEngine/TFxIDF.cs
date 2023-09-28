using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class TFxIDF
    {
        public static Dictionary<string , double> TodasLasPalabras = new Dictionary<string, double>();//Todas las palabras de todos txt
        //Diccionario que contiene todas las palabras de todos los textos sin repetir , con el valor de en cuantos documentos aparace
        public static List<Dictionary<string,double>> DiccionarioDeCadaDocumento = new List<Dictionary<string, double>>();
        //Con tiene los diccionario de cada documento , con el valor de las palabras por TF*IDF



        public static Dictionary<string,double> Calcular_Tf_Idf_query(string query)
        {//Calculando TF-IDF del query
            Dictionary<string,double> vectorquery = new Dictionary<string, double>();
            List<string> WordsOfQuery = Tools.Separar_palabras(query);
            for(int i = 0 ; i < WordsOfQuery.Count ; i++)
            {
                if(!vectorquery.ContainsKey(WordsOfQuery[i]))
                {
                    vectorquery.Add(WordsOfQuery[i] , 1);
                }
                else
                {
                    vectorquery[WordsOfQuery[i]]++;
                }
            }
            foreach(string s in vectorquery.Keys)
            {
                if(TFxIDF.TodasLasPalabras.ContainsKey(s))
                {
                vectorquery[s] = (vectorquery[s] / WordsOfQuery.Count)*(Math.Log10(Tools.textos.Count/TFxIDF.TodasLasPalabras[s]));
                //Calculando tf-idf del vector query
                }
                else vectorquery[s] = 0 ;
            }
            Operators.Priority(vectorquery , query); //Operador de prioridad ***
            return vectorquery;
        } 
    }
}