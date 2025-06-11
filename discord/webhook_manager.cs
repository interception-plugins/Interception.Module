using System;
using System.Threading.Tasks;

using RestSharp;

namespace interception.discord {
    public static class webhook_manager {
        public static void send_webhook(string url, webhook wh) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            if (wh.file_data != null) {
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFileBytes("file.png", wh.file_data, wh.file_name);
                request.AddParameter("payload_json", wh.serialize_json());
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(wh.serialize_json());
            }
            RestClient rc = new RestClient(url);
            rc.Post(request);
        }

        // im really bad at asynchronous programming
        public static async Task send_webhook_async_task(string url, webhook wh) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            if (wh.file_data != null) {
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFileBytes("file.png", wh.file_data, wh.file_name);
                request.AddParameter("payload_json", wh.serialize_json());
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(wh.serialize_json());
            }
            RestClient rc = new RestClient(url);
            await rc.PostAsync<IRestResponse>(request).ConfigureAwait(false);
        }

        public static void send_webhook_async(string url, webhook wh) {
            Task.Run(async () => await send_webhook_async_task(url, wh));
        }
    }
}
