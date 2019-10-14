using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using MTGApro.API.Models;

namespace MTGApro.API
{
    public static class ApiClient
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static void ErrorReport(Exception ex, string token, int lineNo = 0)
        {
            StackTrace st = new StackTrace(ex, true);
            StackFrame frame = st.GetFrame(st.FrameCount - 1);
            int line = frame.GetFileLineNumber();
            int col = frame.GetFileColumnNumber();
            string func = frame.GetMethod().Name;
            string file = frame.GetFileName();

            Dictionary<string, object> report = new Dictionary<string, object>
            {
                {@"cmd", @"cm_errreport"},
                {@"token", token},
                {@"function", func},
                {@"line", line.ToString()},
                {@"col", col.ToString()},
                {@"file", file},
                {@"errmsg", ex.Message},
                {@"version", MainWindow.version.ToString()},
                {
                    @"cm_errreport",
                    "!!!" + lineNo.ToString() + "!!!" + ex.Message + "///" + ex.InnerException + "///" + ex.Source +
                    "///" + ex.StackTrace + "///" + ex.TargetSite + "///" + Environment.OSVersion.Version.Major + "///" +
                    Environment.OSVersion.Version.Minor + "///" + ex.ToString()
                }
            };

            string responseString = MakeRequest(new Uri(@"https://mtgarena.pro/mtg/donew.php"), report, token);
            if (responseString == "ERRCONN")
            {
                try
                {
                    File.AppendAllText(@"upload_err_log.txt", Newtonsoft.Json.JsonConvert.SerializeObject(report));
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    Response info = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(responseString);
                    if (info.Status != @"ok")
                    {
                        try
                        {
                            File.AppendAllText(@"upload_err_log.txt", Newtonsoft.Json.JsonConvert.SerializeObject(report));
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static Dictionary<string, string> VerifyFileChecksum(string token, string filename)
        {
            var result = new Dictionary<string, string>();

            try
            {
                string checksum = CalculateChecksum(filename);

                var response = ApiClient.MakeRequest(
                    new Uri(@"https://mtgarena.pro/mtg/uploadcards.php"),
                    new Dictionary<string, object>
                    {
                        {@"checkmd5", checksum}
                    }, token);

                result.Add("result", response);
                result.Add("file", filename);
                result.Add("md5", checksum);
            }
            catch
            {
                // no nothing
            }

            return result;
        }

        public static void UploadCardData(string token, string cardDataChecksum, string cardDataFile, string cardLocalizationFile)
        {
            var request = new Dictionary<string, object>();
            request.Add("md5", cardDataChecksum);
            request.Add("cards", CompressFile(cardDataFile));
            request.Add("loc", CompressFile(cardLocalizationFile));

            MakeRequest(new Uri(@"https://mtgarena.pro/mtg/uploadcards.php"), request, token);
        }

        public static void UploadEventData(string token, string checksum, string eventLocalizationFile)
        {
            var request = new Dictionary<string, object>();
            request.Add("md5", checksum);
            request.Add("loc", CompressFile(eventLocalizationFile));

            MakeRequest(new Uri(@"https://mtgarena.pro/mtg/mtgalocloader.php"), request, token);
        }

        public static string MakeRequest(Uri uri, Dictionary<string, object> data, string token, string method = "POST")
        {
            try
            {
                string formDataBoundary = string.Format("----------{0:N}", Guid.NewGuid());
                string contentType = "multipart/form-data; boundary=" + formDataBoundary;

                byte[] formData = WriteMultipartForm(data, token, formDataBoundary);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = method;

                if (method == "POST")
                {
                    httpWebRequest.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
                    httpWebRequest.ContentLength = formData.Length;

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        requestStream.Close();
                    }
                }
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                return responseString;
            }
            catch (Exception e)
            {
                try
                {
                    StackTrace st = new StackTrace(e, true);
                    StackFrame frame = st.GetFrame(st.FrameCount - 1);
                    int line = frame.GetFileLineNumber();
                    int col = frame.GetFileColumnNumber();
                    string func = frame.GetMethod().Name;
                    string file = frame.GetFileName();

                    Dictionary<string, object> report = new Dictionary<string, object>
                    {
                        {@"cmd", @"cm_errreport"},
                        {@"token", token},
                        {@"function", func},
                        {@"line", line.ToString()},
                        {@"col", col.ToString()},
                        {@"file", file},
                        {@"errmsg", e.Message},
                        {@"version", MainWindow.version.ToString()},
                        {
                            @"cm_errreport",
                            "!!!" + e.Message + "///" + e.InnerException + "///" + e.Source + "///" + e.StackTrace +
                            "///" + e.TargetSite + "///" + Environment.OSVersion.Version.Major + "///" +
                            Environment.OSVersion.Version.Minor + "///" + e.ToString()
                        }
                    };
                    File.AppendAllText(@"network_err_log.txt", Newtonsoft.Json.JsonConvert.SerializeObject(report));
                }
                catch (Exception)
                {

                }
                return "ERRCONN";
            }
        }

        private static byte[] WriteMultipartForm(Dictionary<string, object> postParameters, string token, string boundary)
        {
            try
            {
                Stream formDataStream = new MemoryStream();
                bool needsCLRF = false;

                foreach (KeyValuePair<string, object> param in postParameters)
                {
                    // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsCLRF)
                        formDataStream.Write(Encoding.GetBytes("\r\n"), 0, Encoding.GetByteCount("\r\n"));

                    needsCLRF = true;

                    if (param.Value is string)
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, param.Key, param.Value);
                        formDataStream.Write(Encoding.GetBytes(postData), 0, Encoding.GetByteCount(postData));
                    }
                    else if (param.Value is byte[])
                    {
                        byte[] writezip = (byte[])param.Value;
                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        param.Key,
                        "application/octet-stream");

                        formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(writezip, 0, writezip.Length);
                    }
                }

                // Add the end of the request.  Start with a newline
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(Encoding.GetBytes(footer), 0, Encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();

                return formData;
            }
            catch (Exception ee)
            {
                ErrorReport(ee, token, 539);
                return null;
            }
        }

        private static string CalculateChecksum(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 32768))
            using (var hasher = new MD5CryptoServiceProvider())
            {
                byte[] hash = hasher.ComputeHash(file);

                StringBuilder result = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                    result.Append(hash[i].ToString("x2"));

                return result.ToString();
            }
        }

        private static byte[] CompressFile(string filename)
        {
            using (var inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var outputStream = new MemoryStream())
            using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                inputStream.CopyTo(compressionStream);
                compressionStream.Flush();
                return outputStream.ToArray();
            }
        }
    }
}