using CommandLine;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.IO;

namespace GoogleCloudStorageAPITest
{
    class Program
    {
        public class Options
        {
            [Option('k', "accesskeyfilepath", Required = true, HelpText = @"存取金鑰檔案路徑: c:/dev-key.json")]
            public string AccessKeyFilePath { get; set; }

            [Option('b', "bucketname", Required = true, HelpText = @"上傳 Bucket Name: dev.king453.com")]
            public string BuecketName { get; set; }

            [Option('u', "uploadfilesrcpath", Required = true, HelpText = @"上傳檔案來源路徑: c:/test.png")]
            public string UploadFilePath { get; set; }

            [Option('p', "uploadpath", Required = true, HelpText = @"上傳路徑: res/user/168.png")]
            public string UploadPath { get; set; }
        }

        static void Main(string[] args)
        {

            Parser
                .Default
                .ParseArguments<Options>(args)
                .WithParsed<Options>(Run);
            //var keyFilePath = @"./qc-key.json";
            //var keyFilePath = @"./phoenix-5-4-d19abb1f86a4.json";

            //// 載入 Google Storage 認證設定檔內容
            //var googleCredential = GoogleCredential.FromFile(keyFilePath);

            //// 建立 Google Storage Client
            //var storageClient = StorageClient.Create(googleCredential);


            //// Storage Bucket 名稱
            //// DEV Bucket: qc.king453.com
            //// QC Bucket: dev.king453.com
            //// PROD Bucket: king108.com

            //// 上傳的路徑檔案名
            //var fileName = "res/user/sdk.png";

            //using (var fileStream = File.OpenRead(@"./sdk.png"))
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        // 讀取檔案內容
            //        fileStream.CopyTo(memoryStream);


            //        // 上傳檔案至 Google Storage Bucket
            //        var a = storageClient.UploadObject(bucketName, fileName, null, memoryStream);
            //        var result = storageClient.UploadObject(bucketName, fileName, null, memoryStream);
            //    }
            //}

            //// 刪除 Google Storage Bucket 檔案
            //storageClient.DeleteObject(bucketName, fileName);
        }


        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="options"></param>
        private static void Run(Options options)
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"存取金鑰檔案路徑: {options.AccessKeyFilePath}");
            Console.WriteLine($"上傳 Bucket Name: {options.BuecketName}");
            Console.WriteLine($"上傳檔案來源路徑: {options.UploadFilePath}");
            Console.WriteLine($"上傳路徑: {options.UploadPath}");
            Console.WriteLine("=================================");
            Console.WriteLine();

            GoogleCredential googleCredential = null;
            StorageClient storageClient = null;
            try
            {
                // 載入 Google Storage 認證設定檔內容
                googleCredential = GoogleCredential.FromFile(options.AccessKeyFilePath);
                Console.WriteLine("載入存取金鑰: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"載入存取金鑰: FAIL, 存取金鑰檔案路徑: {options.AccessKeyFilePath}, 錯誤訊息: {ex.Message}");
                return;
            }

            try
            {
                // 建立 Google Storage Client
                storageClient = StorageClient.Create(googleCredential);
                Console.WriteLine("初始 Google Storage Client: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始 Google Storage Client: FAIL, 存取金鑰檔案路徑: {options.AccessKeyFilePath}, 錯誤訊息: {ex.Message}");
                return;
            }


            // 上傳的路徑檔案名
            var bucketName = options.BuecketName;
            var uploadPath = options.UploadPath;

            // 上傳檔案
            try
            {
                using (var fileStream = File.OpenRead(@"./sdk.png"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // 讀取檔案內容
                        fileStream.CopyTo(memoryStream);


                        // 上傳檔案至 Google Storage Bucket
                        var result = storageClient.UploadObject(bucketName, uploadPath, null, memoryStream);
                    }
                }

                Console.WriteLine($"上傳檔案: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"上傳檔案: FIAL, 錯誤訊息: {ex.Message}");
                return;
            }

            // 取得檔案
            try
            {
                storageClient.GetObject(bucketName, uploadPath);
                Console.WriteLine($"取得檔案: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"取得檔案: FIAL, 錯誤訊息: {ex.Message}");
                return;
            }

            // 刪除檔案
            //try
            //{
            //    storageClient.DeleteObject(bucketName, uploadPath);
            //    Console.WriteLine($"刪除檔案: OK");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"刪除檔案: FIAL, 錯誤訊息: {ex.Message}");
            //    return;
            //}
        }
    }
}
