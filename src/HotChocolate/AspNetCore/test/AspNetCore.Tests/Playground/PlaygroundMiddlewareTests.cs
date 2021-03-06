using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Snapshooter.Xunit;
using Xunit;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore.Tests.Utilities;

namespace HotChocolate.AspNetCore
{
    public class PlaygroundMiddlewareTests
        : ServerTestBase
    {
        public PlaygroundMiddlewareTests(TestServerFactory serverFactory)
            : base(serverFactory)
        {
        }

        [Fact]
        public async Task Default_Values()
        {
            // arrange
            var options = new PlaygroundOptions();

            TestServer server = CreateServer(options);
            string settingsUri = "/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task Disable_Subscriptions()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.EnableSubscription = false;

            TestServer server = CreateServer(options);
            string settingsUri = "/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.Path = "/foo";

            TestServer server = CreateServer(options);
            string settingsUri = "/foo/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetPath_Then_SetQueryPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.Path = "/foo";
            options.QueryPath = "/bar";

            TestServer server = CreateServer(options);
            string settingsUri = "/foo/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetQueryPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.QueryPath = "/foo";

            TestServer server = CreateServer(options);
            string settingsUri = "/foo/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetQueryPath_Then_SetPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.QueryPath = "/foo";
            options.Path = "/bar";

            TestServer server = CreateServer(options);
            string settingsUri = "/bar/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetQueryPath_Then_SetSubscriptionPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.QueryPath = "/foo";
            options.SubscriptionPath = "/bar";

            TestServer server = CreateServer(options);
            string settingsUri = "/foo/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetSubscriptionPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.SubscriptionPath = "/foo";

            TestServer server = CreateServer(options);
            string settingsUri = "/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        [Fact]
        public async Task SetSubscriptionPath_Then_SetQueryPath()
        {
            // arrange
            var options = new PlaygroundOptions();
            options.SubscriptionPath = "/foo";
            options.QueryPath = "/bar";

            TestServer server = CreateServer(options);
            string settingsUri = "/bar/playground/settings.js";

            // act
            string settings_js = await GetSettingsAsync(server, settingsUri);

            // act
            settings_js.MatchSnapshot();
        }

        private TestServer CreateServer(PlaygroundOptions options)
        {
            return ServerFactory.Create(
                services => services.AddStarWars(),
                app => app.UseGraphQL().UsePlayground(options));
        }

        private async Task<string> GetSettingsAsync(
            TestServer server,
            string path)
        {
            HttpResponseMessage response =
                await server.CreateClient().GetAsync(
                    TestServerExtensions.CreateUrl(path));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
