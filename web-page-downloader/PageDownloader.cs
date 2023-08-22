using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using web_page_parser.Data;

namespace web_page_downloader
{
    class PageDownloader
    {
        private HttpClientHandler httpHandler;
        private readonly HttpClient httpClient;

        public PageDownloader()
        {
            this.httpHandler = new HttpClientHandler();
            this.httpClient = new HttpClient(httpHandler);
        }
        public void DownloadPages(List<Tuple<string, string, NetworkCredential, Dictionary<string, string>, Enums.PrinterType>> data)
        {
            try
            {
                Console.WriteLine("Start now.");
                DownloadDataPipeline(data).Wait();
                //Test1().Wait();
                //var res = GetCookieValue().Result;

                //Console.WriteLine("Res: " + res);

                Console.WriteLine("Dl done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                httpHandler.Dispose();
                httpClient.Dispose();
            }

        }
        // TODO - Post auth for CanonLBP6670
        public async Task<bool> Test1()
        {
            var parameters = new Dictionary<string, string> { { "ACTION", "LOGIN" }, { "admin_password", "" }, { "Language", "1" }, { "login_mode", "user" } };
            //var parameters = new Dictionary<string, string> { { "admin_password", "" } };
            var encodedContent = new FormUrlEncodedContent(parameters);


            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://10.6.1.60/portal.cgi?CSUT=-116976889"),
                Method = HttpMethod.Post
            };

            Dictionary<string, string> nHeaders = new Dictionary<string, string>();
            nHeaders.Add("Accept", "text/html, application/xhtml+xml, application/xml; q=0.9, */*; q=0.8");
            nHeaders.Add("Accept-Encoding", "gzip, deflate");
            nHeaders.Add("Accept-Language", "ru-RU");
            nHeaders.Add("Cache-control", "max-age=0");
            nHeaders.Add("Connection", "Keep-Alive");
            nHeaders.Add("Content-Length", "55");
            nHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            nHeaders.Add("Host", "10.6.1.60");
            nHeaders.Add("Referer", "http://10.6.1.60/tlogin.cgi");
            nHeaders.Add("Upgrade-Insecure-Requests", "1");
            nHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362");
            //nHeaders.Add("Cookie", "CookieID=df22417488b25b5af553f957517e302b:");

            foreach (var kv in nHeaders)
            {
                //request.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
            }

            request.Content = new FormUrlEncodedContent(parameters);

            /*
            if (dataSet.Item3 != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", dataSet.Item3.UserName, dataSet.Item3.Password))));
            }
            */

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            //var response = await httpClient.PostAsync(new Uri("http://10.6.1.60"), encodedContent);


            foreach (var val in response.Headers)
            {
                Console.WriteLine(val.Key);

                foreach (var hVal in val.Value)
                {
                    Console.WriteLine(hVal);
                }
            }

            var strCont = await response.Content.ReadAsStringAsync();

            Console.WriteLine(strCont);

            return response.IsSuccessStatusCode;
        }
        private async Task<string> GetCookieValue()
        {
            string url = "http://10.6.1.60/tlogin.cgi";
            var cookieContainer = new CookieContainer();
            var uri = new Uri(url);

            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            })
            {
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    HttpResponseMessage response = await httpClient.GetAsync(uri);
                    var cookie = cookieContainer.GetCookies(uri).Cast<Cookie>().FirstOrDefault();

                    IEnumerable<string> cookies = response.Headers.FirstOrDefault(header => header.Key == "Set-Cookie").Value;

                    Console.WriteLine("Cookie: " + cookies.Count());

                    return cookie?.Value;
                }
            }


        }

        // General pipeline for printer data crawling.
        async Task DownloadDataPipeline(List<Tuple<string, string, NetworkCredential, Dictionary<string, string>, Enums.PrinterType>> data)
        {
            PrintDataParser pdp = new PrintDataParser();

            // we want to execute this in parallel
            var executionOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            // 1.   This block will receive URL and download content, pointed by URL.
            //      IN:     From, Bytes, Destination.
            //      OUT:    From, Bytes, Destination.
            var downloadBlock = new TransformBlock<Tuple<string, string, NetworkCredential, Dictionary<string, string>, Enums.PrinterType>,
                Tuple<string, long, string, Enums.PrinterType>>(
                async dataSet =>
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(dataSet.Item1),
                        Method = HttpMethod.Get
                    };

                    if (dataSet.Item3 != null)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", dataSet.Item3.UserName, dataSet.Item3.Password))));
                    }

                    foreach (var header in dataSet.Item4)
                    {
                        Debug.WriteLine("Add header -> " + header.Key);

                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }

                    var response = await httpClient.SendAsync(request);
                    var totalRead = 0L;
                    var dir = Path.GetDirectoryName(dataSet.Item2);

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                    fileStream = new FileStream(
                        dataSet.Item2,
                        FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        totalRead = 0L;
                        var totalReads = 0L;
                        var buffer = new byte[4096];
                        var isMoreToRead = true;

                        do
                        {
                            var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                            if (read == 0)
                            {
                                isMoreToRead = false;
                            }
                            else
                            {
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;
                                totalReads += 1;

                                //Console.WriteLine(string.Format("total bytes downloaded so far: {0:n0}", totalRead));
                            }
                        }
                        while (isMoreToRead);
                    }

                    return Tuple.Create(dataSet.Item1, totalRead, dataSet.Item2, dataSet.Item5);

                }, executionOptions);

            // 2.   This block will print number of bytes downloaded from specific address to specific destination and transform input data (IN) for next block (OUT). 
            //      IN:     From, Bytes, Destination, Printer type.
            //      OUT:    FileFullPath, Printer type.
            var downloadCompletionBlock = new TransformBlock<Tuple<string, long, string, Enums.PrinterType>, Tuple<string, Enums.PrinterType>>(
                tuple =>
                {
                    Console.WriteLine($"Downloaded {tuple.Item4} {tuple.Item2} bytes from {tuple.Item1} to {tuple.Item3}");
                    return Tuple.Create(tuple.Item3, tuple.Item4);
                });

            // 3.   This block will parse downloaded file.
            var parseBlock = new TransformBlock<Tuple<string, Enums.PrinterType>, ParsedPrinterData>(
                tuple =>
                {
                    return pdp.ParseRawDataFromFile(tuple.Item1, tuple.Item2);
                }, executionOptions);

            // 4.   Clean up block for parsed file.
            var parseBlockOutput = new ActionBlock<ParsedPrinterData>(ppd =>
            {
                Console.WriteLine($"Parsed -> {ppd.TonerLevel} {ppd.DrumLevel} {ppd.NumberOfPages}");

            }, executionOptions);

            // 5.   This block will write data to database.

            // 6.   This block will log result for pipeline iteration.

            // Connect the dataflow blocks to form a pipeline.
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            downloadBlock.LinkTo(downloadCompletionBlock, linkOptions);
            downloadCompletionBlock.LinkTo(parseBlock, linkOptions);
            parseBlock.LinkTo(parseBlockOutput, linkOptions);

            // fill downloadBlock with input data
            foreach (var dataSet in data)
            {
                await downloadBlock.SendAsync(dataSet);
            }

            // Mark the head of the pipeline as complete.
            downloadBlock.Complete();

            // Wait for the last block in the pipeline to process all data.
            await parseBlockOutput.Completion;

            /*
var outputBlock = new ActionBlock<Tuple<string, long, string>>(tuple =>
{
    Console.WriteLine($"Downloaded {tuple.Item2} bytes from {tuple.Item1} to {tuple.Item3}");
}, executionOptions);
*/


            /*
            // here we tell to donwloadBlock, that it is linked with outputBlock;
            // this means, that when some item from donwloadBlock is being processed, 
            // it must be posted to outputBlock
            using (donwloadBlock.LinkTo(outputBlock))
            {
                // fill downloadBlock with input data
                foreach (var dataSet in data)
                {
                    await donwloadBlock.SendAsync(dataSet);
                }

                // tell donwloadBlock, that it is complete; thus, it should start processing its items
                donwloadBlock.Complete();
                // wait while downloading data
                await donwloadBlock.Completion;
                // tell outputBlock, that it is completed
                outputBlock.Complete();
                // wait while printing output
                await outputBlock.Completion;
            }
            */
        }
    }
}
