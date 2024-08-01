// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using ChatGPT.Model;
// using TracksLyrics.Interfaces;
// using TracksLyrics.Models;
// using static TracksLyrics.Settings.Di;
//
// namespace TracksLyrics.Services.Parser;
//
// public class ChatGptParserService : IParserService
// {
//     private int _tryCount; 
//     public Task<TrackLyric> ParsAsync(TrackInfo track)
//     {
//         _tryCount = 0;
//         return ParseSongLyricsAsync(track);
//     }
//
//     private async Task<TrackLyric> ParseSongLyricsAsync(TrackInfo track)
//     {
//         var lrcsResponse = await GetLRC(CreateMessageForLRC(track));
//
//         var timezones = new List<TimeSpan>();
//         var texts = new List<string>();
//
//         if (!lrcsResponse.Success)
//         {
//             Console.WriteLine("Can`t find");
//             return new TrackLyric {
//                 Errors = new List<string> { $"Проблема створення запиту, спробуйте пізніше" }
//             };
//         }
//             
//             
//         try
//         {
//             var lrcs = lrcsResponse.Contents[0].Split('\n').ToList();
//             
//             foreach (var lrc in lrcs)
//             {
//                 var lrcDiv = lrc.Split(']');
//                 var splitMinutes = lrcDiv[0].Split(':');
//                 var splitSeconds = splitMinutes[1].Split('.');
//                 
//                 var minutes = Convert.ToInt32(splitMinutes[0].Split('[')[1]);
//                 
//                 var seconds = Convert.ToInt32(splitSeconds[0]);
//                 var milliseconds = Convert.ToInt32(splitSeconds[1]);
//                 
//                 timezones.Add(new TimeSpan(0, 0, minutes, seconds, milliseconds));
//                 texts.Add(lrcDiv[1]);
//             }
//         }
//         catch
//         {
//             Console.WriteLine("Bad parse");
//             return await ParseSongLyricsAsync(track);
//         }
//             
//         Console.WriteLine($"All okey with {track.Name}");
//             
//         return new TrackLyric {
//             Name = track.Name,
//             TimeZone = timezones,
//             Lyrics = texts,
//         };
//     }
//
//     private async Task<ResponseModel> GetLRC(string message)
//     {
//         _tryCount++;
//         var response = await _senderService.SendMessage(message);
//
//         if (!response.Success || !response.Contents[0].Contains("[00:"))
//         {
//             if (_tryCount == 3)
//             {
//                 Console.WriteLine("TryCount more than 3");
//                 return new ResponseModel {
//                     Success = false
//                 };
//             }
//                 
//             return await GetLRC(message);
//         }
//
//
//         return response;
//     }
//
//     private string CreateMessageForLRC(TrackInfo track)
//         => $"Створи лише LRC {track.Artist} - {track.Name} з таймами рядка такого типу [00:10.33] без лишньої інформації";
// }