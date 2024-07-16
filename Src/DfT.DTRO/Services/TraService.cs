﻿using DfT.DTRO.DAL;
using DfT.DTRO.Models.SwaCode;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="ITraService"/>
/// </summary>
public class TraService : ITraService
{
    private readonly ISwaCodeDal _swaCodeDal;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="swaCodeDal">An <see cref="ISwaCodeDal"/> instance.</param>
    /// 
    public TraService(ISwaCodeDal swaCodeDal)
    {
        _swaCodeDal = swaCodeDal;
    }

    /// <inheritdoc/>
    public async Task<List<SwaCodeResponse>> GetUiFormattedSwaCodeAsync()
    {
        var swaCodeResponses = await _swaCodeDal.GetAllCodes();
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

        return swaCodeResponses;
    }

}