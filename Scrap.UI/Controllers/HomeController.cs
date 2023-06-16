using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Polly;
using Refit;
using Scrap.UI.Models;
using Scrap.UI.Models.DataAccessObject.Wordpress;
using Scrap.UI.Models.DTO;
using Scrap.UI.Models.Wordpress;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Scrap.UI.Controllers
{
    public class ImagePost {
        public int? Id { get; set; }
        public string Url { get; set; }
    }
    public class HomeController : Controller
    {
        String url = "https://www.linkedin.com/login?fromSignIn=true&trk=guest_homepage-basic_nav-header-signin";
        
        public IWebDriver driver;

        List<Job> models = new List<Job>();
        List<string> tags, categories;
        List<ImagePost> images = new List<ImagePost>();
        public async Task<ActionResult> Index()
        {
            //chromeOpen();
            Scrapping();

            //categoryids
            var categoryIds = await getCategories();

            //tagids
            var tagids = await getTags();

            //PostingJob();
            if (models.Count > 0)
            {
                foreach (var model in models)
                {
                    if (!string.IsNullOrEmpty(model.Profile))
                    {
                        await PostingJob(categoryIds, tagids, model);
                    }
                }
            }



            return View();
        }
        

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        private async Task<byte[]> DownloadImageFromWebsiteAsync(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                using (WebResponse response = await request.GetResponseAsync())
                using (var result = new MemoryStream())
                {
                    Stream imageStream = response.GetResponseStream();
                    await imageStream.CopyToAsync(result);
                    return result.ToArray();
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        private void chromeOpen() {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            proc.StartInfo.Arguments = url + " --new-window --remote-debugging-port=9222 --user-data-dir=C:\\Temp";
            proc.Start();
        }
        //public IWebDriver Driver { get; private set; }
        private void Scrapping()
        {

            ChromeOptions options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:9222";

            var policy = Policy
                 .Handle<InvalidOperationException>()
                 .WaitAndRetry(10, t => TimeSpan.FromSeconds(1));

            policy.Execute(() =>
            {
                driver = new ChromeDriver(options);
            });

            string searchText = "remote";
            //driver.Navigate().GoToUrl(string.Format("https://www.linkedin.com/jobs/search/?keywords={0}&refresh=true", searchText));
            //driver.Navigate().GoToUrl(string.Format("https://www.linkedin.com/jobs/search/?currentJobId=3246073042&geoId=102478259&keywords=remote%20developer&location=Indonesia&refresh=true&start=100", searchText));
            //Thread.Sleep(500);

            //find li in jobs ul
            var jobs = driver.FindElements(By.XPath("//ul[@class='scaffold-layout__list-container']"));

            int iLoop = 1;
            foreach (var item in jobs)
            {
                var listItemJob = item.FindElements(By.ClassName("jobs-search-results__list-item"));

                //get pagination 
                //var pagination = driver.FindElements(By.XPath("//ul[@class='artdeco-pagination__pages artdeco-pagination__pages--number']"));
                //foreach (var page in pagination) { 
                    
                //}

                //do looping job item per page
                int waitSecond = 3000;
                int countJob = listItemJob.Count;
                foreach (var job in listItemJob)
                {
                    Thread.Sleep(waitSecond);
                    var model = new Job();
                    try
                    {
                        var jobTitle = job.FindElement(By.ClassName("job-card-list__title"));
                        //var jobTitle = job.FindElement(By.XPath("//div[@class='full-width artdeco-entity-lockup__title ember-view']"));
                        //var jobTitle = job.FindElement(By.ClassName("job-card-container--clickable"));
                        jobTitle.Click();
                        Thread.Sleep(waitSecond);

                        //Thread.Sleep(waitSecond);
                        var jobTitleLink = jobTitle.GetAttribute("href");
                        model.Link = jobTitleLink;
                        model.Title = jobTitle.Text;
                        //var jobHref = jobTitle.FindElement(By.ClassName("job-card-list__title"));
                        //var jobTitleLink = jobHref.GetAttribute("href");
                        //model.Link = jobTitleLink;
                        //model.Title = jobHref.Text;

                        var company = job.FindElement(By.ClassName("job-card-container__company-name"));
                        model.CompanyName = company.Text;

                        var country = job.FindElement(By.ClassName("job-card-container__metadata-item"));
                        var countryName = country.Text;
                        model.Country = countryName;

                        var jobSection = driver.FindElement(By.ClassName("scaffold-layout__detail"));

                        //company and location
                        var companyLocation = jobSection.FindElement(By.ClassName("jobs-unified-top-card__subtitle-primary-grouping"));
                        model.Location = companyLocation.Text;

                        var topCardJobInsights = jobSection.FindElements(By.ClassName("jobs-unified-top-card__job-insight"));
                        var typeTitleSpan = topCardJobInsights[0].FindElement(By.TagName("span"));
                        model.Type = typeTitleSpan.Text;


                        var article = driver.FindElement(By.TagName("article"));
                        model.Description = article.GetAttribute("innerHTML");

                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        //about company
                        try
                        {
                            //remove image company galery if any
                            js.ExecuteScript("return document.getElementsByClassName('jobs-company__content')[0].remove();");
                        }
                        catch (Exception) {}
                        
                        try
                        {
                            //remove show more if any
                            js.ExecuteScript("return document.getElementsByClassName('follow')[0].remove();");
                        }
                        catch (Exception) { }

                        try
                        {
                            //remove show more if any
                            js.ExecuteScript("return document.getElementsByClassName('inline-show-more-text__button')[0].remove();");
                        }
                        catch (Exception) { }

                        try
                        {
                            //remove label on linkedin
                            js.ExecuteScript("return document.getElementsByClassName('jobs-company__inline-information')[1].remove();");
                        }
                        catch (Exception) { }

                        //handle company profile if any
                        try
                        {
                            var companySection = jobSection.FindElement(By.ClassName("jobs-company__box"));
                            var industry = companySection.FindElements(By.XPath("//div[@class='t-14 mt5']"));
                            var industryEmployeeCount = companySection.FindElement(By.XPath("//section/div[1]/div[2]/span[1]"));
                            var about = companySection.FindElement(By.ClassName("inline-show-more-text"));
                            var imageUrl = companySection.FindElement(By.TagName("img"));

                            model.Industry = industry[0].GetAttribute("innerHTML");
                            model.CompanyEmployee = industryEmployeeCount.Text;
                            model.Profile = companySection.GetAttribute("innerHTML").Replace("line-height:2rem;max-height:6rem;", "");
                            model.ImageUrl = imageUrl.GetAttribute("src");
                        }
                        catch (Exception){}

                        models.Add(model);
                    }
                    catch (Exception ex){
                        var xxx = ex.Message;
                    }
                    iLoop++;
                }
            }

            var x = models;
        }

        private async Task<List<int>> getTags() {
            //tag
            TagDAO objTagService = new TagDAO();
            //tags = new List<string>() { "Remote", "Developer", "IT", "Indonesia", "WFH", "Engineer" };
            tags = new List<string>() { "Bank", "Sarjana", "S1", "Diploma", "Indonesia"};
            List<int> tagIds = new List<int>();

            foreach (string item in tags)
            {
                var tag = await objTagService.Create(new WordpressTagRequestDTO()
                {
                    description = "Info Lowongan Kerja " + item,
                    name = item,
                    slug = item.Replace(" ", "-").ToLower()
                });

                tagIds.Add(tag.id);
            }
            return tagIds;
        }
        private async Task<List<int>> getCategories()
        {
            //category
            CategoryDAO objCategoryService = new CategoryDAO();
            categories = new List<string>() { "UMUM"};
            List<int> categoryIds = new List<int>();

            foreach (string item in categories)
            {
                var tag = await objCategoryService.Create(new WordpressCategoryRequestDTO()
                {
                    //description = "Kategori Info Lowongan Kerja " + item,
                    name = item,
                    slug = item.Trim().Replace(" ", "-").ToLower()
                });

                categoryIds.Add(tag.id);
            }
            return categoryIds;
        }
        private async Task PostingJob(List<int> categoryIds, List<int> tagIds, Job model) {
            PostDAO objService = new PostDAO();

            ////media upload
            MediaDAO objMediaService = new MediaDAO();
            var media = new Media();
            var image = images.Where(m => m.Url == model.ImageUrl)?.FirstOrDefault();
            if (images != null && image != null)
            {
                media.id = image.Id;
            }
            else
            {
                string _url = model.ImageUrl;
                var requestMedia = new WordpressMediaRequestDTO()
                {
                    alt_text = "Lowongan Kerja " + model.CompanyName,
                    title = "Info Lowongan Kerja " + model.CompanyName,
                    description = "Informasi Lowongan Kerja bulan ini",
                    caption = "Info Lowongan Kerja bulan ini"
                };
                var fileByte = await DownloadImageFromWebsiteAsync(_url);

                media = await objMediaService.Create(requestMedia, fileByte);
                var _image = new ImagePost() { Id = media.id, Url = _url };
                images.Add(_image);
            }

            //post
            string _prefixTitle = "Lowongan Kerja ";
            string htmlInfolookerAbout = @"<br><br><strong><a href='https://infolooker.id/'>infolooker.id&nbsp;</a></strong>merupakan situs portal Lowongan Kerja di Indonesia, mulai dari Perusahaan&nbsp;<a href='https://infolooker.id/category/lowongan/umum/' target='_blank' rel='noreferrer noopener'>Swasta</a>,&nbsp;<a href='https://lokerterkini.com/category/loker-bumn/' target='_blank' rel='noreferrer noopener'>BUMN</a>, hingga informasi&nbsp;<a href='https://infolooker.id/category/lowongan/cpns/' target='_blank' rel='noreferrer noopener'>CPNS</a>&nbsp;dengan kategori dari&nbsp;<a href='https://infolooker.id/tag/sma/' target='_blank' rel='noreferrer noopener'>SMA/Sederajat</a>,&nbsp;<a href='https://infolooker.id/tag/diploma/' target='_blank' rel='noreferrer noopener'>D3</a>, hingga&nbsp;<a href='https://infolooker.id/tag/sarjana/' target='_blank' rel='noreferrer noopener'>S1</a>/<a href='https://infolooker.id/tag/sarjana/' target='_blank' rel='noreferrer noopener'>S2</a>. Informasi lowongan kerja di&nbsp;<a href='https://infolooker.id/' target='_blank' rel='noreferrer noopener'>infolooker.id</a>&nbsp;bersumber dari website resmi perusahaan dan sosial media yang terpercaya yang sedang membuka kesempatan karir dan menjadi bagian dari perusahaan.</p><br><br>";
            string htmlDaftar = @"<br><br>Jika kamu tertarik dengan posisi <strong>" + model.Title.ToLower() + "</strong> ini, kamu bisa langsung mendaftar/apply secara online dengan klik DAFTAR dibawah ini :<br><strong><a href='" + model.Link + " ' target='_blank' rel='noopener' title=''>DAFTAR</a></strong>";
            string htmlMoreLoker = @"<p>Belum dapat lowongan kerja " + tags.FirstOrDefault().ToLower() + " yang sesuai? yuk cek lowongan kerja lainnya disini : <strong><a href='https://infolooker.id/tag/" + tags.FirstOrDefault().ToLower() + "'  target='_blank' rel='noopener' title=''>LOWONGAN KERJA " + tags.FirstOrDefault().ToUpper() + "</a></strong></p>";


            string title = _prefixTitle + model.CompanyName ;
            WordpressPostRequestDTO request = new WordpressPostRequestDTO()
            {
                title = title + " - " + model.Title,
                content = model.Profile + "<h3>" + title + " - " + model.Location + "</h3>"  + model.Description + htmlDaftar + htmlInfolookerAbout + htmlMoreLoker,
                //status = "draft",
                status = "publish",
                categories = categoryIds,
                tags = tagIds,
                featured_media = media?.id
            };
            var response = await objService.Create(request);
            var x = response;
        }

        private IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            while (true)
            {
                var elements = driver.FindElements(by);
                if (elements.Count > 0)
                    return elements;
                Thread.Sleep(10);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

}