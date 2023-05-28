using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime;


namespace MoogleEngine 
{
    public static class Tools
    {
        public static char SeparadorDelSistema = Path.DirectorySeparatorChar;
        public static string[] DireccionTextos = Directory.GetFiles(".."+SeparadorDelSistema+"Content"+SeparadorDelSistema , "*.txt");
        //Obteniendo la dirección de mis textos
        public static DirectoryInfo ForNames = new DirectoryInfo("../Content/");
        //Obteniendo dirección para obtener los nombres de mis textos
        public static List<string> textos = new List<string>();
        //Lista de mis textos listos ya leídos (Snippet)
        public static List<string> FilesNames = new List<string>();
        //Lista de los nombres de los textos
        public  static List<List<string>> Text_Words = new List<List<string>>();
        //Lista que contiene Las listas de palabras tokenizadas de cada texto
        public static void GetValues()
        { 
           /*
           Método inicializador : Lee los textos , Obtiene todas las palabras , los nombres de los textos , los diccionarios de cada 
           documento , y calcula TFxIDF de las palabras de cada documento.
           */
           // File.ReadAllText(directorio);
            for(int i = 0 ; i < DireccionTextos.Length ; i++)//Este for es lo q más se demora
            {
                textos.Add(File.ReadAllText(DireccionTextos[i]));//Obteniendo todos mis textos
                List<string> words = Separar_palabras(textos[i]);
                Text_Words.Add(Separar_palabras(textos[i]));
                TFxIDF.DiccionarioDeCadaDocumento.Add(new Dictionary<string, double>());

                for(int j = 0 ; j < words.Count ; j++)
                {
                    //Obteniendo los diccionarios de cada documento
                    if(!TFxIDF.DiccionarioDeCadaDocumento[i].ContainsKey(words[j]))
                    {
                        TFxIDF.DiccionarioDeCadaDocumento[i].Add(words[j] , 1);//Añadiendo las palabras sin repetirse
                        
                        //Una vez verifica que la palabra aparece por primera vez en el documento actual , añado mis palabras al diccionario de todas las palabras
                        if(!TFxIDF.TodasLasPalabras.ContainsKey(words[j]))
                        {
                            TFxIDF.TodasLasPalabras.Add(words[j] , 1);
                        
                        }
                        else
                        {
                            TFxIDF.TodasLasPalabras[words[j]]++;
                            
                        }
                    }
                    else
                    {
                        TFxIDF.DiccionarioDeCadaDocumento[i][words[j]]++;//Cantidad de veces que se repite la palabra por cada doc(TF)
                    }
                }
                int TotalDePalabrasDelDocumento = Text_Words[i].Count;//Cuantas palabras tiene el texto
                foreach(string s in TFxIDF.DiccionarioDeCadaDocumento[i].Keys)
                {
                    //i representa mi documento actual y s la palabra del documento
                    TFxIDF.DiccionarioDeCadaDocumento[i][s] = (TFxIDF.DiccionarioDeCadaDocumento[i][s] / TotalDePalabrasDelDocumento)*(Math.Log10(Tools.textos.Count/TFxIDF.TodasLasPalabras[s]));
                    //Calculo del TF*IDF
                }

            }

            FileInfo[] files = ForNames.GetFiles("*.txt");//Obteniendo todos los nombres de los textos
            foreach(FileInfo f in files)
            {
                FilesNames.Add(f.Name);
            }
        }

        //Método para tomar todas las palabras de un texto por separado
        public static List<string> Separar_palabras(string texto) 
        {
            string[] words ;
            texto = Regex.Replace(texto, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]", " ");//using System.Text.RegularExppressions
            texto = texto.ToLower();
            words = texto.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return words.ToList<string>();
        }

    }

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
            string result = "";
            string text = Tools.textos[index_of_text].ToLower();
            text = Regex.Replace( text, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]", " ");
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

     public class Operators
    {
        public static List<string> DontShowWord(string query)
        {
            string[] words ;
            List<string> result = new List<string>();
            query = Regex.Replace(query, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ!]", " ");//Añadiendo signo de exclamación
            query = query.ToLower();              
            words = query.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0 ; i < words.Length ; i++)
            {
                string temp = words[i];
                if(temp[0] == '!')
                {
                    temp = Regex.Replace(temp, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]", "");
                    result.Add(temp);//Añado a esta lista todas las palabras que tienen !
                }
            }
            return result;
        }

        public static List<string> ImportantWord(string query)
        {
            string[] words ;
            List<string> result = new List<string>();
            query = Regex.Replace(query, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ^]", " ");//Añadiendo signo de ^
            query = query.ToLower();              
            words = query.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0 ; i < words.Length ; i++)
            {
                string temp = words[i];
                if(temp[0] ==  '^')
                {
                    temp = Regex.Replace(temp, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]", "");
                    result.Add(temp);//Añado a esta lista todas las palabras que tienen ^
                }
            }
            return result;
        }

        public static void Priority(Dictionary<string , double> v , string q)
        {
            string[] words ;
            List<string> result = new List<string>();
            q = Regex.Replace(q, @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ*]", " ");//Añadiendo signo de *
            q = q.ToLower();              
            words = q.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0 ; i < words.Length ; i++)
            {

                for(int j = 0 ; j < words[i].Length ; j++)
                {
                    string temp =  Regex.Replace(words[i], @"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ]", "");

                    if(words[i][j] == '*')
                    {
                        if(v.ContainsKey(temp))
                        {
                            v[temp] = v[temp] * 2;
                        }
                    }
                    else break;
                }
            }
        }
        
    }
    
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

    public class Similitud_Coseno
    {
        public double Calcular_Similitud_Coseno(Dictionary<string,double> query , Dictionary<string,double> doc)
        {
            double MultiplicacionDeVectores = 0 ;
            double ModuloVectorQuery = 0 ;
            double ModuloVectorDoc = 0 ;
            foreach (string s in query.Keys)
            {
                ModuloVectorQuery += Math.Pow(query[s] , 2);
                if(doc.ContainsKey(s))
                {
                MultiplicacionDeVectores += query[s]*doc[s];
                }

            }
            foreach(string s in doc.Keys)
            {
                ModuloVectorDoc += Math.Pow(doc[s] , 2);
            }
            ModuloVectorQuery = Math.Sqrt(ModuloVectorQuery);
            ModuloVectorDoc = Math.Sqrt(ModuloVectorDoc);
            if(ModuloVectorQuery*ModuloVectorDoc != 0 ){
                return (MultiplicacionDeVectores) / (ModuloVectorQuery * ModuloVectorDoc);
            }else return 0; //Evitar que tenga valor NaN   
        }
        
        public List<(string , string , float , int)> ScoreTop(string query){//Retorna lista de documentos con score      
            List<(string , string , float , int)> ScoreTxT = new List<(string , string , float , int)>();
            Dictionary<string,double> queryTfIdf = TFxIDF.Calcular_Tf_Idf_query(query);
            for(int i = 0 ; i < Tools.textos.Count ; i++)
            {
                float score = (float)Calcular_Similitud_Coseno(queryTfIdf , TFxIDF.DiccionarioDeCadaDocumento[i]);

                 List<string> NoWords = Operators.DontShowWord(query);// Operador !
                foreach(string s in NoWords)
                {
                    if(TFxIDF.DiccionarioDeCadaDocumento[i].ContainsKey(s))//Si la palabra está el socre es 0
                    {
                        score = 0;
                    }
                }

                List<string> ImportantWords = Operators.ImportantWord(query);//Operador ^
                foreach(string s in ImportantWords)
                {
                    if(!TFxIDF.DiccionarioDeCadaDocumento[i].ContainsKey(s))//Si la palabra no está el score es 0
                    {
                        score = 0;
                    }
                }
                
                if(score != 0){
                    ScoreTxT.Add((Tools.FilesNames[i] , " En Desarrollo (snipet) " , score , i));//Guardo i para representar mi doc a la hora de calcular el snippet    
                }    
            }
            //Ordenar de mayor a menor.
            int ScoreTxtLength = ScoreTxT.Count;
            while(ScoreTxtLength > 0){
                for(int i = ScoreTxT.Count-1 ; i > 0 ; i--){
                    (string , string , float , int) x ;//Variable temporal
                    if(ScoreTxT[i-1].Item3 <  ScoreTxT[i].Item3){
                        x = (ScoreTxT[i-1]);
                        ScoreTxT[i-1] = ScoreTxT[i];
                        ScoreTxT[i] = x;
                    }
                }
                ScoreTxtLength--;
            }
            return ScoreTxT; 
        }
    }  
}
