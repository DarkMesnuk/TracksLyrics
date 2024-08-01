using System.Text.RegularExpressions;
using Common.Base.Interfaces.Requests;
using Common.Schemas;
using Newtonsoft.Json;
using Serilog;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Repository.File.Repositories;

public class TrackLyricsFileRepository : ITrackLyricsRepository
{
    private const string FilePath = @"D:\Source\TrackLyrics.json";
    private readonly List<TrackLyricModel> _trackLyrics;

    public TrackLyricsFileRepository()
    {
        _trackLyrics = new List<TrackLyricModel>();

        var exists = System.IO.File.Exists(FilePath);
        if (!exists)
            using (System.IO.File.Create(FilePath))
            { }

        using var file = new StreamReader(FilePath);
        
        var json = file.ReadToEnd();

        if (!string.IsNullOrEmpty(json))
        {
            var parsedList = JsonConvert.DeserializeObject<List<TrackLyricModel>>(json);
            
            if(parsedList != null)
                _trackLyrics = parsedList;
        }
        
        _trackLyrics = _trackLyrics.OrderBy(trackLyric => trackLyric.Name).ToList();
        
        //ClearAllCyrillicTracks().GetAwaiter().GetResult();
        
        PrintStatisticsOfTracks();
    }

    public bool Exists(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TrackLyricModel> CreateAsync(TrackLyricModel trackLyric)
    {
        if (IsCyrillicTrack(trackLyric))
        {
            Log.Debug("Error for operation {AsyncName} - value {TrackLyric}. Track is Cyrillic", nameof(CreateAsync), trackLyric);
            return new TrackLyricModel();
        }

        trackLyric.Id = _trackLyrics.Count + 1;
        
        _trackLyrics.Add(trackLyric);

        var isSuccess = await UpdateFileAsync();

        if (!isSuccess)
        {
            Log.Debug("Error for operation {AsyncName} - value {TrackLyric}", nameof(CreateAsync), trackLyric);
            return new TrackLyricModel();
        }
        
        return trackLyric;
    }

    public Task<bool> CreateAsync(IEnumerable<TrackLyricModel> models)
    {
        throw new NotImplementedException();
    }

    public Task<TrackLyricModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IPaginatedResponseSchema<TrackLyricModel>> GetAllAsync(IPaginatedRequest schema)
    {
        throw new NotImplementedException();
    }

    public Task<TrackLyricModel?> GetByInfoOrDefaultAsync(TrackInfoModel trackInfo)
    {
        var trackLyric = _trackLyrics.FirstOrDefault(trackLyric =>
            trackLyric.Name == trackInfo.Name && trackLyric.Artist == trackInfo.Artist);
        
        return Task.FromResult(trackLyric);
    }

    public Task<TrackLyricModel> UpdateLyricsAsync(TrackLyricModel trackLyric)
    {
        throw new NotImplementedException();
    }

    public async Task<TrackLyricModel> UpdateAsync(TrackLyricModel trackLyric)
    {
        var trackLyricOld = await GetByInfoOrDefaultAsync(new TrackInfoModel { Name = trackLyric.Name, Artist = trackLyric.Artist });

        if (trackLyricOld == null)
        {
            Log.Debug("Error for operation {AsyncName} - value {TrackLyric}, can`t find track in file", nameof(UpdateAsync), trackLyric);
            return new TrackLyricModel();
        }

        _trackLyrics.Remove(trackLyricOld);
        
        trackLyric.Id = _trackLyrics.Count + 1;
        
        _trackLyrics.Add(trackLyric);
        
        var isSuccess = await UpdateFileAsync();

        if (!isSuccess)
        {
            Log.Debug("Error for operation {UpdateAsyncName} - value {TrackLyric}", nameof(UpdateAsync), trackLyric);
            return new TrackLyricModel();
        }

        return trackLyric;
    }

    public Task<bool> UpdateRangeAsync(IEnumerable<TrackLyricModel> models)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TrackLyricModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRangeAsync(IEnumerable<TrackLyricModel> models)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> UpdateFileAsync()
    {
        await using var file = new StreamWriter(FilePath);
        
        var json = JsonConvert.SerializeObject(_trackLyrics);
        string errorMessage;

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                await file.WriteLineAsync(json);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        else
        {
            errorMessage = "Can`t serialize tracks list";
        }
        
        Log.Debug("Error to update file (Error: {ErrorMessage})", errorMessage);
        return false;
    }

    private bool IsCyrillicTrack(TrackLyricModel track)
    {
        return Regex.IsMatch(track.Name, @"[а-яА-ЯіїєІЇЄ]");
    }

    private void PrintStatisticsOfTracks()
    {
        Log.Debug("Усього пройшли пошук - {TrackLyricsCount} треків", _trackLyrics.Count);
        
        if (_trackLyrics.Count == 0)
            return;
        
        Log.Debug($"З них");
        
        var tracksFiltered = _trackLyrics
            .Count(x => x.IsFinded && x.IsTranslated);

        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Мають оригінал і переклад", tracksFiltered, _trackLyrics.Count);
        
        tracksFiltered = _trackLyrics
            .Count(x => x.IsFinded && !x.IsTranslated);
        
        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Мають лише оригінал", tracksFiltered, _trackLyrics.Count);
        
        tracksFiltered = _trackLyrics
            .Count(x => !x.IsFinded);
        
        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Не знайшли інформацію", tracksFiltered, _trackLyrics.Count);
        
        var trackLyricsAfterCyrillicFilter = _trackLyrics.Where(trackLyric => !IsCyrillicTrack(trackLyric)).ToList();
        
        Log.Debug("Усього пройшли пошук без врахування пісень українською - {Count} треків", trackLyricsAfterCyrillicFilter.Count);
        
        Log.Debug($"З них");
        
        tracksFiltered = trackLyricsAfterCyrillicFilter
            .Count(x => x.IsFinded && x.IsTranslated);
        
        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Мають оригінал і переклад", tracksFiltered, trackLyricsAfterCyrillicFilter.Count);
        
        tracksFiltered = trackLyricsAfterCyrillicFilter
            .Count(x => x.IsFinded && !x.IsTranslated);
        
        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Мають лише оригінал", tracksFiltered, trackLyricsAfterCyrillicFilter.Count);
        
        tracksFiltered = trackLyricsAfterCyrillicFilter
            .Count(x => !x.IsFinded);
        
        if (tracksFiltered != 0)
            LogPrintStatisticsOfTracks("Не знайшли інформацію", tracksFiltered, trackLyricsAfterCyrillicFilter.Count);
    }

    private void LogPrintStatisticsOfTracks(string message, int tracksFiltered, int additionalCount)
    {
        if (_trackLyrics.Count == 0 || additionalCount == 0)
            return;
        
        Log.Debug("{TracksFiltered}/{TrackLyricsCount} - {Message} ({AdditionalCount})%)", tracksFiltered, _trackLyrics.Count, message, tracksFiltered/additionalCount);
    }
}