using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using EAISSignalRPoc.Helpers;
using EAISSignalRPoc.Controllers;

namespace EAISSignalRPoc.Hubs
{
    public class EaisHub: Hub
    {
        private readonly EngagmentReportGenerator engagmentReportGenerator;
        private bool raisedHandStopped=false;
        private bool particpantStopped=false;
        private HubsCommon hubsCommon;

        //private string current 

        public EaisHub(EngagmentReportGenerator engagmentReportGenerator, HubsCommon hubsCommon)
        {
            Console.WriteLine("in constructor");
            this.engagmentReportGenerator = engagmentReportGenerator;
            this.hubsCommon = hubsCommon;
        }

        public async Task GetLiveEngagmentReport()
        {
            Console.WriteLine("in EaisHub.GetLiveEngagmentReport");
            Task.Run(() => hubsCommon.SendLiveEngagmentReport(Context.ConnectionId));
        }

        // with paramters...



        public async Task Stop(string stoped)
        {
            Console.WriteLine("in stop " + Context.ConnectionId);
            if (stoped.Equals("connection", StringComparison.OrdinalIgnoreCase))
            {
                Context.Abort();
            }

            else
            {
                hubsCommon.StopNotificationsForSpecificAttribute(stoped, Context.ConnectionId);
            }

        /*
        public async Task GetLiveEngamentReportTask()
        {
            Console.WriteLine(Context.ConnectionId);
            DateTime start = DateTime.Now;
            EngagmentReport result;
            do
            {
                result = engagmentReportGenerator.GetUpdate("in start" + Context.ConnectionId);
                await Task.Delay(1000);
                if (!hubsCommon.ParticpantStopped & !hubsCommon.RaisedHandStopped)
                {
                    await Clients.Caller.SendAsync("EngagmentReportUpdate", result);
                }
                else
                {
                    if (hubsCommon.ParticpantStopped)
                    {
                        await Clients.Caller.SendAsync("raisedHandsUpdate", result.RaiseHandsCnt);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("ParticpantsUpdate", result.RaiseHandsCnt);
                    }
                }
            } while (DateTime.Now < start.AddMinutes(0.25));
        }
        */

        /*
        public async Task Start(string firstInsight, string secondInsight)
        {
            Task.Run(async () =>
            {
                DateTime start = DateTime.Now;
                EngagmentReport engagmentReport;
                do
                {
                    engagmentReport = engagmentReportGenerator.GetUpdate(Context.ConnectionId);
                    await Task.Delay(1000);
                    if (!particpantStopped && (firstInsight.Equals("particpants", StringComparison.OrdinalIgnoreCase) || secondInsight.Equals("particpants", StringComparison.OrdinalIgnoreCase)))
                    {
                        await Clients.Caller.SendAsync("ParticpantsUpdate", engagmentReport.ParticpantCnt);
                    }


                    if (!raisedHandStopped && (secondInsight.Equals("raisedHands", StringComparison.OrdinalIgnoreCase) || firstInsight.Equals("raisedHands", StringComparison.OrdinalIgnoreCase)))
                    {
                        await Clients.Caller.SendAsync("raisedHandsUpdate", engagmentReport.RaiseHandsCnt);
                    }

                } while (DateTime.Now < start.AddMinutes(0.25));
            });
        }
        */
        }
    }
}
