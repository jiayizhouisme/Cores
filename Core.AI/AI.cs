using RestSharp;

namespace Core.AI
{
    public class AI
    {
        public async Task AITest()
        {
            var client = new RestClient($"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer bce-v3/ALTAK-2MxubGsJUVMI5ANjpxJr3/8798ca7322ff3aec40d55aa98a45a30a2fe58c1b");
            var body = @"{""messages"":[{""role"":""user"",""content"":""ces""}],""temperature"":0.95,""top_p"":0.8,""penalty_score"":1,""enable_system_memory"":false,""disable_search"":false,""enable_citation"":false,""response_format"":""text""}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
