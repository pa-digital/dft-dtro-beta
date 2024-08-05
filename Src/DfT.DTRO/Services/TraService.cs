using System.Dynamic;
using System;
using System.Globalization;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.SwaCode;
using System.Collections.Generic;


namespace DfT.DTRO.Services;

public class TraService : ITraService
{
    private readonly ISwaCodeDal _swaCodeDal;

    public TraService(ISwaCodeDal swaCodeDal)
    {
        _swaCodeDal = swaCodeDal;
    }

    private List<SwaCodeResponse> FormatTraNameForUi(List<SwaCodeResponse> swaCodeResponses)
    {
        var ret = new List<SwaCodeResponse>();
        foreach (var swaCode in swaCodeResponses)
        {
          ret.Add(FormatTraNameForUi(swaCode));
        }

        return ret;
    }

    private SwaCodeResponse FormatTraNameForUi(SwaCodeResponse swaCode)
    {
        var sb = new StringBuilder();

        string[] words = swaCode.Name.Split(' ');

        bool isFirstWordInBrackets = false;
        if (words.Length > 0 && words[0].StartsWith("(") && words[0].EndsWith(")"))
        {
            isFirstWordInBrackets = true;
        }

        if (words.Length > 0)
        {
            if (!isFirstWordInBrackets && words[0].Length >= 4)
            {
                words[0] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[0].ToLower());
            }

            sb.Clear();
            sb.Append(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                sb.Append(' ');
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower()));
            }
            swaCode.Name = sb.ToString();
        }
        return swaCode;
    }

    public async Task<List<SwaCodeResponse>> SearchSwaCodes(string partialName)
    {
        var swaCodeResponses = await _swaCodeDal.SearchSwaCodesAsync(partialName);
        swaCodeResponses = FormatTraNameForUi(swaCodeResponses);
        return swaCodeResponses;
    }

    public async Task<List<SwaCodeResponse>> GetSwaCodeAsync()
    {
        var swaCodeResponses = await _swaCodeDal.GetAllCodesAsync();
        swaCodeResponses = FormatTraNameForUi(swaCodeResponses);
        return swaCodeResponses;
    }

    public async Task<SwaCodeResponse> GetSwaCodeAsync(int traId)
    {
        var swaCodeResponse = await _swaCodeDal.GetSwaCodeAsync(traId);
        swaCodeResponse = FormatTraNameForUi(swaCodeResponse);
        return swaCodeResponse;
    }

    public async Task<GuidResponse> ActivateTraAsync(int traId)
    {
        var traExists = await _swaCodeDal.TraExistsAsync(traId);
        if (!traExists)
        {
            throw new NotFoundException("TRA not found");
        }

        return await _swaCodeDal.ActivateTraAsync(traId);
    }

    public async Task<GuidResponse> DeActivateTraAsync(int traId)
    {
        var traExists = await _swaCodeDal.TraExistsAsync(traId);
        if (!traExists)
        {
            throw new NotFoundException("TRA not found");
        }

        return await _swaCodeDal.DeActivateTraAsync(traId);
    }

    public async Task<GuidResponse> SaveTraAsync(SwaCodeRequest swaCodeRequest)
    {
        var traExists = await _swaCodeDal.TraExistsAsync(swaCodeRequest.TraId);
        if (traExists)
        {
            throw new InvalidOperationException("TRA already Exists");
        }

        return await _swaCodeDal.SaveTraAsync(swaCodeRequest);
    }

    public async Task<GuidResponse> UpdateTraAsync(SwaCodeRequest swaCodeRequest)
    {
        var traExists = await _swaCodeDal.TraExistsAsync(swaCodeRequest.TraId);
        if (!traExists)
        {
            throw new NotFoundException();
        }

        return await _swaCodeDal.UpdateTraAsync(swaCodeRequest);
    }
}