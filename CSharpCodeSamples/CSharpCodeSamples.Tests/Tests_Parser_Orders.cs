namespace CSharpCodeSamples.Tests
{
    using System.Linq;
    using Common.Interfaces.Models;
    using Domain.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests;

    [TestClass]
    public class Tests_Parser_Orders : Tests_Base
    {
        private IUserRequest   _userRequest;
        private IParsedRequest _doesRequest;


        /// <summary>
        /// I'm not sharing the unit tests at this time because they are proprietary,
        /// but I will try to create general audience equivalents in the coming days.
        /// Below is just a very simple example.
        /// </summary>

        [TestMethod]
        public void TestParser01() {
            IUserContext ucon = new UserContext();
            ucon.InitializeValues("agent", "url", "user name");

            _userRequest = ServicesForTheRequest.BuildTheUserRequest()
                                                .WithUserContext(ucon)
                                                .ForEntity("OR")
                                                .UseCommandLine(" John Smith ");
            _doesRequest = ServicesForTheRequest.ParseThisRequest(_userRequest);

            Assert.IsTrue(_doesRequest.EntityName.Equals("ORDER"), "Incorrect scope");
            Assert.IsTrue(_doesRequest.HaveCrossFieldSearchItems, "Failed to load cross field search items");
            Assert.IsTrue(_doesRequest.CrossFieldSearchItems.Count().Equals(2), "Incorrect number of cross field search items identified");
            Assert.AreEqual("JOHN", _doesRequest.CrossFieldSearchItems.First().SearchValue);
            Assert.AreEqual(SearchFieldOperators.Default, _doesRequest.CrossFieldSearchItems.First().Operator);
            Assert.AreEqual("SMITH", _doesRequest.CrossFieldSearchItems.Skip(1).First().SearchValue);
            Assert.AreEqual(SearchFieldOperators.Default, _doesRequest.CrossFieldSearchItems.Skip(1).First().Operator);
            Assert.IsTrue(_doesRequest.ParseErrors.Count.Equals(0), "Invalid - parse errors found");
            //--------------------------------------------------------------------------------------------------------
        }
    }
}
