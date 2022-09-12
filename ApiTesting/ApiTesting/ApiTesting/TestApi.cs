using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    [TestClass]
    public class TestApi
    {

        [TestMethod]
        public async Task ShouldReturn200Ok()
        {
            var uri = new Uri("https://api.publicapis.org/entries");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri;

            var response = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, "?title=cat")).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var entries = JsonConvert.DeserializeObject<EntriesModel>(body);

            Assert.IsNotNull(entries, "Content should not be null");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Unexpected status code");
        }

        [TestMethod]
        public async Task ShouldReturn200OkWithNullContent()
        {
            var uri = new Uri("https://api.publicapis.org/entries");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri;

            var response = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, "?title=dadad")).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var entries = JsonConvert.DeserializeObject<EntriesModel>(body);

            Assert.AreEqual(entries.Count, 0, "Unexpected entries count");
            Assert.IsNull(entries.Entries, "Content should not be null");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Unexpected status code");
        }

        [TestMethod]
        public async Task ShouldReturn400BadRequestWhenQueryIsInvalid()
        {
            var invalidQuery = "invalidQuery";
            var uri = new Uri("https://api.publicapis.org/entries");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri;

            var response = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, $"?{invalidQuery}")).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.IsTrue(body.Contains($"{invalidQuery}"), "Content should not be null");
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Unexpected status code");
        }
    }

    public class EntriesModel
    {
        public int Count { get; set; }
        public List<EntryModel> Entries { get; set; }
    }

    public partial class EntryModel
    {

        public string Api { get; set; }

        public string Description { get; set; }

        public string Auth { get; set; }

        public bool Https { get; set; }

        public string Cors { get; set; }

        public Uri Link { get; set; }

        public string Category { get; set; }
    }
}
