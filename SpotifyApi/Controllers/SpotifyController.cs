using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace SpotifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        static bool IsPause = false;
        [HttpPost("nextTrack")]
        public async Task<IActionResult> NextTrackAsync()
        {
            var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(new TokenAuthenticator(Config.AccessToken, "Bearer"));
            var spotify = new SpotifyClient(config);

            try
            {
                await spotify.Player.SkipNext(new PlayerSkipNextRequest()); 
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e);
            }
            
            return Ok();
        }

        [HttpPost("previousTrack")]
        public async Task<IActionResult> PreviousTrackAsync()
        {
            var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(new TokenAuthenticator(Config.AccessToken, "Bearer"));
            var spotify = new SpotifyClient(config);
            
            try
            {
                await spotify.Player.SkipPrevious(new PlayerSkipPreviousRequest());
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e);
            }
            
            return Ok();
        }

        [HttpPost("pause/resume")]
        public async Task<IActionResult> PauseAsync()
        {
            var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(new TokenAuthenticator(Config.AccessToken, "Bearer"));
            var spotify = new SpotifyClient(config);

            try
            {
                if(IsPause)
                    await spotify.Player.ResumePlayback(new PlayerResumePlaybackRequest());
                else
                    await spotify.Player.PausePlayback(new PlayerPausePlaybackRequest());
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e);
            }

            IsPause = !IsPause;

            return Ok();
        }
    }
}