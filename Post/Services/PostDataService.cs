using Newtonsoft.Json;
using Post.Models;

namespace Post.Services
{
    public class PostDataService
    {
        private readonly HttpClient _httpClient;

        public PostDataService()
        {
            _httpClient = new HttpClient();
        }

        public class PostResponse
        {
            // Represents a response containing a list of PostData objects
            public List<PostData> Posts { get; set; } 
        }

        public async Task<List<PostData>> GetPostDataAsync(string tag, string? direction, string? sortBy)
        {
            try
            {
                // Split the tags by comma and trim whitespace
                var tagValues = tag?.Split(',').Select(tag => tag.Trim());

                // Use a HashSet to automatically remove duplicate PostData objects
                var postDataHashSet = new HashSet<PostData>();

                // Iterate over each tag
                foreach (var _tag in tagValues)
                {
                    // Construct the API query URL with the tag
                    var queryParams = $"https://api.hatchways.io/assessment/blog/posts?tag={_tag}";

                    // Make an HTTP GET request to the API
                    HttpResponseMessage response = await _httpClient.GetAsync(queryParams);
                    response.EnsureSuccessStatusCode(); // Ensure the request was successful
                    string jsonResponse = await response.Content.ReadAsStringAsync(); // Read the response JSON

                    // Deserialize the JSON response into a PostResponse object
                    var postResponse = JsonConvert.DeserializeObject<PostResponse>(jsonResponse);

                    // Add each PostData object to the HashSet
                    foreach (var data in postResponse.Posts)
                    {
                        postDataHashSet.Add(data);
                    }
                }

                // Convert the HashSet to a List
                var postDataList = postDataHashSet.ToList();

                // Order the posts based on the specified field and direction
                switch (sortBy?.ToLower())
                {
                    case "reads":
                        postDataList = direction?.ToLower() == "desc"
                            ? postDataList.OrderByDescending(p => p.Reads).ToList()
                            : postDataList.OrderBy(p => p.Reads).ToList();
                        break;
                    case "likes":
                        postDataList = direction?.ToLower() == "desc"
                            ? postDataList.OrderByDescending(p => p.Likes).ToList()
                            : postDataList.OrderBy(p => p.Likes).ToList();
                        break;
                    case "popularity":
                        postDataList = direction?.ToLower() == "desc"
                            ? postDataList.OrderByDescending(p => p.Popularity).ToList()
                            : postDataList.OrderBy(p => p.Popularity).ToList();
                        break;
                    default:
                        postDataList = direction?.ToLower() == "desc"
                            ? postDataList.OrderByDescending(p => p.Id).ToList()
                            : postDataList.OrderBy(p => p.Id).ToList();
                        break;
                }

                return postDataList; // Return the ordered list of PostData objects
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling API: {ex.Message}"); // Handle HTTP request error
            }
        }
    }
}
