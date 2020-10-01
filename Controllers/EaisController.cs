using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EAISSignalRPoc.Helpers;
using EAISSignalRPoc.Hubs;

namespace EAISSignalRPoc.Controllers
{
    //[Route("[controller]")]
    public class EaisController : Controller
    {
        private readonly IHubContext<EaisHub> eaisHubContext;

        private EngagmentReportGenerator engagmentReportGenerator;

        private HubsCommon hubsCommon;


        public void SendMessageToClient(string funcName , string param)
        {
            eaisHubContext.Clients.All.SendAsync(funcName, param);
        }



    }
}
