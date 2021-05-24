using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace DotNetRestSharp
{
    public class Tests
    {
        private WireMockServer _server;

        [SetUp]
        public void Setup()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public void Should_respond_to_mock_request()
        {
            var host = "http://localhost:";
            var path = "/test";
            var message = @"{ ""msg"": ""Hello world!"" }";

            _server
              .Given(Request.Create().WithPath(path).UsingGet())
              .RespondWith(
                Response.Create()
                  .WithStatusCode(200)
                  .WithBody(message)
              );

            var client = new RestClient(host + _server.Ports[0]);
            var request = new RestRequest(path, DataFormat.Json);
            var response = client.Get(request);

            response.Content.Should().Contain("Hello world");
            response.Content.Should().BeEquivalentTo(message);
        }



        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}