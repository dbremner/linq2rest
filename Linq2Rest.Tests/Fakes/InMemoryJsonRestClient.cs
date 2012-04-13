using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Linq2Rest.Parser;
using Linq2Rest.Provider;

namespace Linq2Rest.Tests.Fakes {
    public class InMemoryJsonRestClient<T> : IRestClient {
        private readonly IEnumerable<T> data;
        private DataContractJsonSerializer serializer;

        public InMemoryJsonRestClient(IEnumerable<T> data, IEnumerable<Type> knownTypes) {
            this.data = data;
            serializer = new DataContractJsonSerializer(typeof (T), knownTypes);
        }

        public void Dispose() {
            
        }

        public Uri ServiceBase {
            get { return new Uri("http://localhost/InMemoryClient/"); }
        }

        public string Get(Uri uri) {
            string content;
            using (var stream = new MemoryStream()) {
                serializer.WriteObject(stream, data.Filter(HttpUtility.ParseQueryString(uri.Query)).ToArray());
                stream.Seek(0, SeekOrigin.Begin);
                content = Encoding.Default.GetString(stream.ToArray());
            }
            return content;
        }
    }
}