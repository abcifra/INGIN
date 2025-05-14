//using System;
//using System.IO;
//using System.Net;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Octokit;


//namespace Updater
//{
//    public class Updater
//    {
//        static void Main()
//        {
//            try
//            {
//                var client = new GitHubClient(new ProductHeaderValue("RevitPluginUpdater"));
//                var releases = client.Repository.Release.GetAll("abcifra", "INGIN").Result;

//                if (releases.Count == 0)
//                {
//                    Console.WriteLine("Нет доступных релизов.");
//                    return;
//                }

//                var latestRelease = releases[0];
//                if (latestRelease.Assets.Count == 0)
//                {
//                    Console.WriteLine("Нет файлов для скачивания.");
//                    return;
//                }

//                string downloadUrl = latestRelease.Assets[0].BrowserDownloadUrl;
//                string fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
//                string localPath = Path.Combine(Path.GetTempPath(), fileName);

//                DownloadFile(downloadUrl, localPath);
//                Console.WriteLine("Обновление загружено!");

//                InstallUpdate(localPath);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }

//        static void DownloadFile(string url, string destinationPath)
//        {
//            try
//            {
//                using (WebClient webClient = new WebClient())
//                {
//                    webClient.DownloadFile(url, destinationPath);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка загрузки файла: {ex.Message}");
//            }
//        }

//        static void InstallUpdate(string msiPath)
//        {
//            var process = new Process();
//            process.StartInfo.FileName = "msiexec.exe";
//            process.StartInfo.Arguments = $"/i \"{msiPath}\" /quiet /norestart";
//            process.StartInfo.UseShellExecute = false;
//            process.StartInfo.CreateNoWindow = true;
//            process.Start();
//            process.WaitForExit();
//            var a = "sdhjkhksd";
//            if (process.ExitCode == 0)
//            {
//                Console.WriteLine("Обновление завершено успешно!");
//            }
//            else
//            {
//                Console.WriteLine($"Ошибка при установке, код завершения: {process.ExitCode}");
//            }
//        }
//    }
//}

using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Updater
{
    public class Updater
    {
        static async Task Main()
        {
            try
            {
                string repoOwner = "abcifra";
                string repoName = "INGIN";
                string apiUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("RevitPluginUpdater");
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Ошибка получения данных о релизе.");
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var root = doc.RootElement;
                        if (!root.TryGetProperty("assets", out JsonElement assets) || assets.GetArrayLength() == 0)
                        {
                            Console.WriteLine("Нет доступных файлов.");
                            return;
                        }

                        string downloadUrl = assets[0].GetProperty("browser_download_url").GetString();
                        string fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
                        string localPath = Path.Combine(Path.GetTempPath(), fileName);

                        DownloadFile(downloadUrl, localPath);
                        Console.WriteLine("Обновление загружено!");

                        InstallUpdate(localPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DownloadFile(string url, string destinationPath)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, destinationPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки файла: {ex.Message}");
            }
        }

        static void InstallUpdate(string msiPath)
        {
            var process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.Arguments = $"/i \"{msiPath}\" /quiet /norestart";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Console.WriteLine("Обновление завершено успешно!");
            }
            else
            {
                Console.WriteLine($"Ошибка при установке, код завершения: {process.ExitCode}");
            }
        }
    }
}
