namespace MoogleEngine
{
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
        
        public List<(string , string , float , int)> ScoreTop(string query)
        {//Retorna lista de documentos con score      
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

                //Operador ~
                score += (float)Operators.ClosedWords(query , i);
                
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
                        x = ScoreTxT[i-1];
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