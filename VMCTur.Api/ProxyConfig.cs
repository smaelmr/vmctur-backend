using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace VMCTur.Api
{
    public class ProxyConfig : IWebProxy
    {
        public ICredentials Credentials
        {
            get { return new NetworkCredential("smunari@senacrs.com.br", "qwe123"); }
            //or get { return new NetworkCredential("user", "password","domain"); }
            set { }
        }

        public Uri GetProxy(Uri destination)
        {
            return new Uri("http://172.16.100.231:8080");
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
