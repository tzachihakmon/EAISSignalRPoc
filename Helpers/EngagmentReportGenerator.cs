using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAISSignalRPoc.Helpers
{
    public class EngagmentReportGenerator
    {
        Dictionary<string, EngagmentReport> EngagmentReports = new Dictionary<string, EngagmentReport>();
        private readonly Random random;

        public EngagmentReportGenerator(Random random)
        {
            this.random = random;
        }
        public async Task<EngagmentReport> GetUpdate(string meetingId)
        {
            if (EngagmentReports.TryGetValue(meetingId, out EngagmentReport engagmentReport))
            {
                engagmentReport.ParticpantCnt += random.Next(0, 5);
                for (int i=0; i< engagmentReport.ParticpantCnt; i++)
                {
                    if (random.Next(1, 20) == 4)
                    {
                        engagmentReport.RaiseHandsCnt += 1;
                    }
                }
                return engagmentReport;
            }
            else
            {
                EngagmentReports.Add(meetingId, new EngagmentReport
                {
                    ParticpantCnt = 0,
                    RaiseHandsCnt = 0,
                });
                return EngagmentReports[meetingId];
            }
        }
    }


    public class EngagmentReport
    {
        public int ParticpantCnt { get; set; }

        public int RaiseHandsCnt { get; set; }
    }
}