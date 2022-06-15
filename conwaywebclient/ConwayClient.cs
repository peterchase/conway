using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConwayLib;
using ConwayWebModel;
using System.Text.Json;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace ConwayWebClient
{
    public class ConwayClient
    {
        HttpClient mClient = new HttpClient();
        const string ROOT = "Board/";
        string mBaseAddress;
        
        public async Task<IEnumerable<BoardInfo>> GetBoardsAsync()
        {
            IEnumerable<BoardInfo> boards = null;
            HttpResponseMessage response = await mClient.GetAsync(ROOT);
            if (response.IsSuccessStatusCode)
            {
                boards = await JsonSerializer.DeserializeAsync<IEnumerable<BoardInfo>>(await response.Content.ReadAsStreamAsync());
            }
            return boards;
        }
        public async Task<BoardDetail> GetBoardDetailAsync(int id)
        {
            return await GetBoardDetailAsync($"{ROOT}{id}");
        }
        
        public async Task<BoardDetail> GetBoardDetailAsync(string url)
        {
            BoardDetail state = null;
            HttpResponseMessage response = await mClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            state = await response.Content.ReadAsAsync<BoardDetail>();

            if (state == null) {throw new Exception("Could not deserialize Board Json.");}
            return state;
        }

        public async Task<Uri> CreateBoardAsync(BoardDetail detail)
        {
            HttpResponseMessage response = await mClient.PostAsJsonAsync(ROOT,detail);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public async Task<HttpStatusCode> DeleteBoardAsync(int id)
        {
            HttpResponseMessage response = await mClient.DeleteAsync(ROOT+id.ToString());
            return response.StatusCode;
        }

        public ConwayClient(string baseAddress)
        {
            // Update port # in the following line.
            mBaseAddress = baseAddress;
            mClient.BaseAddress = new Uri(mBaseAddress);
            mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }        
    }
}