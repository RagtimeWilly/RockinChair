using NUnit.Framework;
using System.Linq;
using System.Net.Http;

namespace RockinChair.Tests
{
    [TestFixture]
    public class ClientTests
    {
        private SpotifyClient _spotify;

        [TestFixtureSetUp]
        public void Setup()
        {
            _spotify = new SpotifyClient(() => new HttpClient());
        }

        [Test]
        public void SearchTracks_TracksExist_ReturnsTracks()
        {
            var result = _spotify.SearchTracks("The Band", "The Weight").Result;

            Assert.Greater(result.Count(), 0);
        }
    }
}
