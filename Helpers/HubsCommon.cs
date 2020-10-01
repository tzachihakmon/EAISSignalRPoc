using EAISSignalRPoc.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAISSignalRPoc.Helpers
{

    public class HubsCommon
    {

        private readonly IHubContext<EaisHub> hubContext;
        private EngagmentReportGenerator engagmentReportGenerator;
        private ConcurrentDictionary<string, StopIndicators> connectionIdToStopIndicators = new ConcurrentDictionary<string, StopIndicators>();

        public HubsCommon(IHubContext<EaisHub> hubContext, EngagmentReportGenerator engagmentReportGenerator)
        {
            this.hubContext = hubContext;
            this.engagmentReportGenerator = engagmentReportGenerator;
        }

        public async Task SendLiveEngagmentReport(string connectionId)
        {
            Console.WriteLine("in HubsCommon");
            if (!connectionIdToStopIndicators.TryGetValue(connectionId ,out StopIndicators stopIndicators))
            {
                stopIndicators = new StopIndicators();
                if(!connectionIdToStopIndicators.TryAdd(connectionId, stopIndicators))
                {
                    Console.WriteLine("in HubsCommon : failed to save id dict: " + connectionId);

                }
                await hubContext.Groups.AddToGroupAsync(connectionId, connectionId);
                Console.WriteLine("in HubsCommon : saved group for id: "+ connectionId);
            }

            //this.eaisHubContext.Clients.All(connectionId);
            DateTime start = DateTime.Now;
            EngagmentReport result;
            do
            {
                result = await engagmentReportGenerator.GetUpdate(connectionId);
                await Task.Delay(1000);
                if (!stopIndicators.ParticpantStopped & !stopIndicators.RaisedHandStopped)
                {
                    await hubContext.Clients.Group(connectionId).SendAsync("EngagmentReportUpdate", result);
                }
                else
                {
                    if (stopIndicators.ParticpantStopped)
                    {
                        if (!stopIndicators.RaisedHandStopped)
                        {
                            await hubContext.Clients.Group(connectionId).SendAsync("raisedHandsUpdate", result.RaiseHandsCnt);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        await hubContext.Clients.Group(connectionId).SendAsync("ParticpantsUpdate", result.ParticpantCnt);
                    }
                }
            } while (DateTime.Now < start.AddMinutes(0.5));
            stopIndicators.InitIndicators();
        }

        public async Task StopNotificationsForSpecificAttribute(string attributeName , string connectionId)
        {
            if(!connectionIdToStopIndicators.TryGetValue(connectionId, out StopIndicators stopIndicators)){
                Console.WriteLine("in HubsCommon.StopNotificationsForSpecificAttribute : failed ti get stopIndicators for connectionId: " + connectionId);
                return;
            }
            else if (attributeName.Equals("particpants", StringComparison.OrdinalIgnoreCase))
            {
                stopIndicators.ParticpantStopped = true;
            }
            else if (attributeName.Equals("raisedHands", StringComparison.OrdinalIgnoreCase))
            {
                stopIndicators.RaisedHandStopped = true;
            }
            else
            {
                Console.WriteLine("in HubsCommon.StopNotificationsForSpecificAttribute : bad attributeName " + attributeName);
            }
        }

        public class StopIndicators
        {
            public bool RaisedHandStopped = false;
            public bool ParticpantStopped = false;

            public void InitIndicators()
            {
                RaisedHandStopped = false;
                ParticpantStopped = false;
            }
        }
    }

}
