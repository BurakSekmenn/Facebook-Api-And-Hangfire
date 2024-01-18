using HangFire.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Hangfire;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HangFire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        private string userAccessToken = "your_acces_token";
        private string pageId = "your_page_id";
 
        [HttpPost]
        public async Task<IActionResult> VideoAsync(string message, string photoUrls)
        {
            var photoUrlList = photoUrls.Split(',').Select(url => url.Trim()).ToList();

            var photoIds = await UploadPhotosToFacebook(userAccessToken, photoUrlList);
           

            if (photoIds.Count > 0)
            {
                var success = await PublishPhotosToPage(userAccessToken, photoIds, message);

                if (success)
                {
                   
                    
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("message", "Failed to post photos to Facebook.");
                }
            }
            else
            {
                ModelState.AddModelError("photoUrls", "No photos were uploaded.");
            }

            return View("Index");
        }
      
        private async Task<List<string>> UploadPhotosToFacebook(string userAccessToken, List<string> imageUrls)
        {
            var uploadedPhotoIds = new List<string>();

            foreach (var imageUrl in imageUrls)
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiUrl = $"https://graph.facebook.com/v18.0/{pageId}/photos";

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(userAccessToken), "access_token");
                formData.Add(new StringContent("0"), "published"); // Set this to 1 for published photos
                formData.Add(new StringContent(imageUrl), "url");


                var response = await httpClient.PostAsync(apiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var photoId = JObject.Parse(responseData)["id"].ToString();
                    uploadedPhotoIds.Add(photoId);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }

            return uploadedPhotoIds;
        }
        [HttpPost]
        public IActionResult ScheduleFacebookPost(string message, string photoUrls, string userAccessToken, DateTime scheduledTime)
        {
            BackgroundJob.Schedule(() => VideoAsync(message, photoUrls), scheduledTime);
            return RedirectToAction("Index");
        }
        private async Task<bool> PublishPhotosToPage(string userAccessToken, List<string> photoIds, string message)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiUrl = $"https://graph.facebook.com/v18.0/{pageId}/feed";

                var postData = new
                {
                    message = message,
                    attached_media = photoIds.Select(id => new { media_fbid = id }).ToArray(),

                    access_token = userAccessToken
                };

                var json = JsonConvert.SerializeObject(postData);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                return response.IsSuccessStatusCode;

            }

        }
     
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}