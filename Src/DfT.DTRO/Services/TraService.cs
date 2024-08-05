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

    private void FormatTraNameForUi(ref List<SwaCodeResponse> swaCodeResponses)
    {
        var sb = new StringBuilder();
        foreach (var swaCode in swaCodeResponses)
        {
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
        }
    }

    public async Task<List<SwaCodeResponse>> SearchSwaCodes(string partialName)
    {
        var swaCodeResponses = await _swaCodeDal.SearchSwaCodesAsync(partialName);
        FormatTraNameForUi(ref swaCodeResponses);
        return swaCodeResponses;
    }

    public async Task<List<SwaCodeResponse>> GetSwaCodeAsync()
    {
        var swaCodeResponses = await _swaCodeDal.GetAllCodesAsync();
        FormatTraNameForUi(ref swaCodeResponses);
        return swaCodeResponses;
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