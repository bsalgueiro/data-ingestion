using System;
using System.Net;

namespace DataIngestion.TestAssignment.DataManager
{
    public interface IWebClient : IDisposable
    {
        WebClient GetWebClient();
    }
}