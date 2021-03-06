﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Appceptive.Agent.Core
{
    public class ApiClient
    {
        public async Task CreateActivity(string application, Activity activity)
		{
			using(var client = CreateApiClient())
			{
			    var applicationId = await GetApplicationId(application);
				var url = string.Format("/applications/{0}/activities", applicationId);
				
				var response = await client.PostAsJsonAsync(url, activity);
				response.EnsureSuccessStatusCode();
			}
		}

        private async Task<string> GetApplicationId(string name)
        {
            using (var client = CreateApiClient())
            {
                var response = await client.PostAsJsonAsync("/applications", new { Name = name });

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    response = await client.GetAsync(response.Headers.Location);
                    response.EnsureSuccessStatusCode();
                }

                var application = await response.Content.ReadAsAsync<dynamic>();
                return application.id;
            }
        }

		private HttpClient CreateApiClient()
		{
			var baseAddress = new Uri(Appceptive.Configuration.ApiUrl);
			var client = new HttpClient {BaseAddress = baseAddress};

			client.DefaultRequestHeaders.Add("X-Api-Key", Appceptive.Configuration.ApiKey);

			return client;
		}
    }
}