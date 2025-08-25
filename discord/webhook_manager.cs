using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;

using interception.discord.types;

namespace interception.discord {
    public static class webhook_manager {
        public static IRestResponse send_webhook(string url, webhook wh) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            var len = wh.files.Count;
            if (len > 0) {
                request.AddHeader("Content-Type", "multipart/form-data");
                for (int i = 0; i < len; i++)
                    request.AddFileBytes($"file{i}", wh.files[i].data, wh.files[i].name);
                request.AddParameter("payload_json", wh.serialize_json_data());
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(wh.serialize_json_data());
            }
            RestClient rc = new RestClient(url);
            return rc.Post(request);
        }

        public static IRestResponse send_webhook(string url, string json_data, List<file_attachment> files = null) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            var len = files != null ? files.Count : 0;
            if (len > 0) {
                request.AddHeader("Content-Type", "multipart/form-data");
                for (int i = 0; i < len; i++)
                    request.AddFileBytes($"file{i}", files[i].data, files[i].name);
                request.AddParameter("payload_json", json_data);
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(json_data);
            }
            RestClient rc = new RestClient(url);
            return rc.Post(request);
        }

        // im really bad at asynchronous programming
        public static async Task<IRestResponse> send_webhook_async(string url, webhook wh) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            var len = wh.files.Count;
            if (len > 0) {
                request.AddHeader("Content-Type", "multipart/form-data");
                for (int i = 0; i < len; i++)
                    request.AddFileBytes($"file{i}", wh.files[i].data, wh.files[i].name);
                request.AddParameter("payload_json", wh.serialize_json_data());
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(wh.serialize_json_data());
            }
            RestClient rc = new RestClient(url);
            return await rc.PostAsync<IRestResponse>(request).ConfigureAwait(false);
        }

        public static async Task<IRestResponse> send_webhook_async(string url, string json_data, List<file_attachment> files = null) {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            var len = files != null ? files.Count : 0;
            if (len > 0) {
                request.AddHeader("Content-Type", "multipart/form-data");
                for (int i = 0; i < len; i++)
                    request.AddFileBytes($"file{i}", files[i].data, files[i].name);
                request.AddParameter("payload_json", json_data);
            }
            else {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(json_data);
            }
            RestClient rc = new RestClient(url);
            return await rc.PostAsync<IRestResponse>(request).ConfigureAwait(false);
        }

        public static void send_webhook_async_unsafe(string url, webhook wh) {
            Task.Run(async () => await send_webhook_async(url, wh));
        }

        public static void send_webhook_async_unsafe(string url, string json_data, List<file_attachment> files = null) {
            Task.Run(async () => await send_webhook_async(url, json_data, files));
        }
    }
}
