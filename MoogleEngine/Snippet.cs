using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class SnippetClass
    {
        public static string BestWord(Dictionary<string,double> query , int index)//Para el Snippet
        {
            //Busco la palabra de mi query con mejor score en el documento[index] , y esta será la mejor palabra de mi query respecto al texto
            (string,double) best_word = ("x" , 0);
            string result = "";
            
            foreach(string s in query.Keys)
            {
                if(TFxIDF.DiccionarioDeCadaDocumento[index].ContainsKey(s))
                {
                    if(TFxIDF.DiccionarioDeCadaDocumento[index][s] > best_word.Item2)
                    {
                        best_word = (s , TFxIDF.DiccionarioDeCadaDocumento[index][s]);
                    }
                }    
            }
            result = best_word.Item1;
            return  result;
        }

        public static string snippet(string BestWord , int index_of_text)
        {
            string result = "" ;
            string text = Tools.textos[index_of_text].ToLower();
            text = Regex.Replace( text, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]" , " ");
            int index_Of_BestWord ;

            
            if(text.IndexOf(BestWord) == 0 && !char.IsLetterOrDigit(text[BestWord.Length]) )//Verificando que sea la primera palabra del texto
            {
                index_Of_BestWord = text.IndexOf(BestWord);
            }
            else if(text.Contains(' ' + BestWord + ' '))
            {
                index_Of_BestWord = text.IndexOf(' ' + BestWord + ' ') + 1; //Sumo uno para no tomar el espacio como índice
            }
            else 
            index_Of_BestWord = text.IndexOf(' ' + BestWord);

            if(index_Of_BestWord == - 1) index_Of_BestWord = 0 ;

            //Creando rango de caracteres hacia la izquierda y derecha
            int count = 70;
            
            while(index_Of_BestWord - count < 0 || text[index_Of_BestWord-count] != ' ')//Evitando que mi rango se vaya del rango del texto y que corte las palabras
            {
                count--;
                if(index_Of_BestWord - count == 0) break;
            }

            for(int i = index_Of_BestWord - count ; i < index_Of_BestWord ; i++)
            {
                result += Tools.textos[index_of_text][i];  //textos[index_of_text]
            }

            if(count == 0 )
            {
                count = 140 ;
            }
            else count = 70;
            
            for(int j = index_Of_BestWord ; j < Tools.textos[index_of_text].Length ; j++)//Creando rango para la derecha del snippet
            {
                if(count <= 0 && text[j] == ' ') break;//Parar cuando llegue al rango del snippet y evitar q corte la palabra
                result += Tools.textos[index_of_text][j];
                count--;
            }

            return result;
        }
    }
}