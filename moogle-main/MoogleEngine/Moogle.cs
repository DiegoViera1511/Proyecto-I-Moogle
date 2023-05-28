namespace MoogleEngine;


public static class Moogle
{
    
    public static SearchResult Query(string query){
        // Modifique este método para responder a la búsqueda
         
        
        Similitud_Coseno SC = new Similitud_Coseno();
        List<(string , string , float , int)> result = SC.ScoreTop(query);
        Dictionary<string,double> VectorQuery = TFxIDF.Calcular_Tf_Idf_query(query);
        string suggestion = SuggestionClass.Suggestion(VectorQuery);
       
        SearchItem[] items ;
        
        if(result.Count > 5)
        {
            items = new SearchItem[5];
            for(int i = 0 ; i < 5 ; i++)
            {
                result[i] = ( result[i].Item1 , SnippetClass.snippet(SnippetClass.BestWord( VectorQuery, result[i].Item4) ,result[i].Item4 ) , result[i].Item3 ,result[i].Item4); 
                items[i] = new SearchItem(result[i].Item1 , result[i].Item2 , result[i].Item3);
            }
            return new SearchResult(items , suggestion);
        }
        else if(result.Count != 0)
        {
            items = new SearchItem[result.Count];
                for(int i = 0 ; i < result.Count ; i++)
                {
                    result[i] = ( result[i].Item1 , SnippetClass.snippet(SnippetClass.BestWord(VectorQuery , result[i].Item4) ,result[i].Item4 ) , result[i].Item3 ,result[i].Item4); 
                    items[i] = new SearchItem(result[i].Item1 , result[i].Item2 , result[i].Item3);
                }
                return new SearchResult(items , suggestion);
        }
        else
        {
            items = new SearchItem[1]
            {
            new SearchItem("No se han encontrado resultados para su búsqueda" , "" , 0.9f)
            };
            return new SearchResult(items , suggestion);
        }   
    }
}
