using System.Text.RegularExpressions;

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
            for(int i = 0 ; i < DireccionTextos.Length ; i++)
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
                    //Cantidad de veces que aparece la palabra en el texto / el total de palabras del documento (TF) * log10 de la cantidad de textos / la cantidad de textos en los que aparece la palabra (IDF)
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
}