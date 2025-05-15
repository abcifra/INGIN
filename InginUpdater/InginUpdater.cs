using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace InginUpdater
{
    public class InginUpdater
    {
        //static void Main()
        //{
        //    Console.WriteLine("sdfksfjckdsjfkdfdsfsfssssdmf");
        //    Console.ReadLine();
        //}
        static void Main()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("RevitPluginUpdater"));
                var releases = client.Repository.Release.GetAll("abcifra", "INGIN").Result;

                if (releases.Count == 0)
                {
                    Console.WriteLine("Нет доступных релизов.");
                    return;
                }

                var latestRelease = releases[0];
                if (latestRelease.Assets.Count == 0)
                {
                    Console.WriteLine("Нет файлов для скачивания.");
                    return;
                }

                string downloadUrl = latestRelease.Assets[0].BrowserDownloadUrl;
                string fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
                string localPath = Path.Combine(Path.GetTempPath(), fileName);

                DownloadFile(downloadUrl, localPath);
                Console.WriteLine("Обновление загружено!");

                InstallUpdate(localPath);
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
            var a = "sdhjsdsdkhksagfgd";
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
