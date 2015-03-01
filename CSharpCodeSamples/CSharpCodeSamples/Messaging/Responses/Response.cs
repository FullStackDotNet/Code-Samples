namespace CSharpCodeSamples.Messaging.Responses
{
    using System.Collections.Generic;
    using System.Linq;

    using Common.Interfaces.Messaging.Requests;
    using Common.Interfaces.Messaging.Responses;
    using Common.Interfaces.Models;

    public class Response : IResponse
    {
        private IMixedResult[] _results;

        public string EntityName { get; private set; }

        public bool           ActionSuccess { get; set; }
        public List<string>   ParseErrors   { get; set; }
        public int            TotalItems    { get { return (Results == null) ? 0 : Results.Count(); } }

        //* Ordered Results
        public IMixedResult[] Results
        {
            get
            {
                switch (EntityName)
                {
                    //case "ICONNECT":
                    //    return _results == null ? null : _results.Cast<iConnectOrder>().OrderBy(ob => ob.OrderId).ToArray();
                    //case "ORDER":
                    //    return _results == null ? null : _results.Cast<Order>().OrderByDescending(ob => ob.OrderDate).ThenBy(ob => ob.OrderNumber).ToArray();
                    //case "TIMEENTRY":
                    //    return _results == null ? null : _results.Cast<TimeEntry>().OrderBy(ob => ob.ClientName).ThenByDescending(ob => ob.Date).ToArray();
                    default:
                        return _results;
                }
            }
            set { _results = value; }
        }

        public Response()
        {
            ParseErrors = new List<string>();
            Results     = null;
        }

        internal Response(IParsedRequest request, IMixedResult[] results)
        {
            EntityName  = request.EntityName;
            Results     = results;
            ParseErrors = request.ParseErrors;
        }

        internal void AddResults(IMixedResult[] list)
        {
            Results = Results == null ? list : Results.Union(list).ToArray();
        }

    }
}
