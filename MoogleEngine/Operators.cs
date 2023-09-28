using System.Text.RegularExpressions;

namespace MoogleEngine
{
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

        public static double ClosedWords(string query , int indexOfText)
        {
            query = Regex.Replace(query ,@"[^\sa-zA-Z0-9áéíóúüÁÉÍÓÚÜöÖñÑ~]", " " );
            List<Match> matches = Regex.Matches(query , @"\w+|~").ToList();
            List<string> words = new List<string>();
            Dictionary<string , List< int >> WordIndexs = new Dictionary<string, List< int >>();
            foreach(Match m in matches)
            {
                words.Add(m.Value);
            }

            if( ! words.Contains("~") ) return 0 ;
            
            Dictionary<string , int > WordsPositions = new Dictionary<string , int>();

            for(int i = 0 ; i < words.Count ; i++)
            {
                if(words[i] != "~")
                {
                    WordIndexs.Add(words[i] , new List<int>());
                    for(int j = 0 ; j < Tools.Text_Words[indexOfText].Count ; j++)
                    {
                        if(Tools.Text_Words[indexOfText][j] == words[i])
                        {   
                            WordIndexs[words[i]].Add(j);
                        }   
                    }
                }
            }
            double minDistance = int.MaxValue;
            for(int i = 0 ; i < words.Count ; i++)
            {
                if(words[i] == "~")
                {
                    if( i - 1 >= 0 && WordIndexs.ContainsKey(words[i-1]))
                    {
                        if(i+1 < words.Count && WordIndexs.ContainsKey(words[i+1]))
                        {
                            foreach(int a in WordIndexs[words[i-1]])
                            {
                                foreach(int b in WordIndexs[words[i+1]])
                                {
                                    double tempMinDistance = Math.Abs(a - b);

                                    if(tempMinDistance < minDistance )
                                    {
                                        minDistance = tempMinDistance ;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(minDistance == int.MaxValue)
            {
                return 0 ;
            }
            else return 10d/minDistance ;
        }
    }
}