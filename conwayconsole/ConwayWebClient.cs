using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConwayLib;
using ConwayWebModel;
using System.Text.Json;
using static System.Math;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace ConwayConsole
{
    public static class ConwayWebClient
    {
        static HttpClient mClient = new HttpClient();
        const string ROOT = "Board/";
        const string BASE_ADDRESS = "https://localhost:5001/";
        
        public static async Task<IEnumerable<BoardInfo>> GetBoardsAsync()
        {
            IEnumerable<BoardInfo> boards = null;
            HttpResponseMessage response = await mClient.GetAsync(ROOT);
            if (response.IsSuccessStatusCode)
            {
                boards = await JsonSerializer.DeserializeAsync<IEnumerable<BoardInfo>>(await response.Content.ReadAsStreamAsync());
            }
            return boards;
        }
        public static async Task<BoardDetail> GetBoardDetailAsync(int id)
        {
            BoardDetail state = null;
            HttpResponseMessage response = await mClient.GetAsync(ROOT + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                state = await JsonSerializer.DeserializeAsync<BoardDetail>(await response.Content.ReadAsStreamAsync());
            }
            return state;
        }

        static async Task<Uri> CreateProductAsync(BoardDetail detail)
        {
            HttpResponseMessage response = await mClient.PostAsJsonAsync(ROOT,detail);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(int id)
        {
            HttpResponseMessage response = await mClient.DeleteAsync(ROOT+id.ToString());
            return response.StatusCode;
        }

        public static void SetupClient()
        {
            // Update port # in the following line.
            mClient.BaseAddress = new Uri(BASE_ADDRESS);
            mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }        
    }
}