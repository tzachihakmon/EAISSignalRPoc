/* unabled commuinication techniqes*/

//WebSocket = undefined;
//EventSource = undefined;


let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalRPoc") //, signalR.HttpTransportType.LongPolling
        .build();
    // when new update about the order arrive i'm updatding the status.


    connection.on("EngagmentReportUpdate", (engagmentReport) => {
        const raiseHandCnt = document.getElementById("raiseHandsCnt");
        const particpantsCnt = document.getElementById("particpantsCnt");

        raiseHandCnt.value = engagmentReport.raiseHandsCnt;
        particpantsCnt.value = engagmentReport.particpantCnt;
    }
    );

    connection.on("ParticpantsUpdate", (particpantUpdate) => {
        const particpantsCnt = document.getElementById("particpantsCnt");
        particpantsCnt.value = particpantUpdate;
    }
    );

    connection.on("RaisedHandsUpdate", (raisedHandsUpdate) => {
        const particpantsCnt = document.getElementById("raiseHandsCnt");
        particpantsCnt.value = raisedHandsUpdate;
    }
    );

    connection.on("MeetingEnded", function () {
            connection.stop(); // or Context.abort in server-side
        }
    );

    connection.start()
        .catch(err => console.error(err.toString())); 
};
setupConnection();

document.getElementById("submitLiveEngagmentReport").addEventListener("click", e => {
    e.preventDefault();

    connection.invoke("GetLiveEngagmentReport");

});

document.getElementById("stopParticpants").addEventListener("click", e => {
    e.preventDefault();
    connection.invoke("Stop", "particpants");

});

document.getElementById("stopRaisedHands").addEventListener("click", e => {
    e.preventDefault();
    connection.invoke("Stop", "raisedHands");
});

document.getElementById("stopConnectionByServer").addEventListener("click", e => {
    e.preventDefault();
    connection.invoke("Stop", "connection");
});

document.getElementById("submitSpecificTypes").addEventListener("click", e => {
    e.preventDefault();

    const firstInsight = document.getElementById("firstInsight").value;
    const secondInsight = document.getElementById("secondInsight").value;

    connection.invoke("Start", firstInsight, secondInsight)

});

