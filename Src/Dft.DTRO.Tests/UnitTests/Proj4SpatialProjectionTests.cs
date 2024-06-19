using DfT.DTRO.Services.Conversion;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class Proj4SpatialProjectionTests
{
    const double ErrorMarginPercent = 1.2;

    [Theory]
    [InlineData(-0.1272, 51.5074, 530068.1072096, 180380.3079476)]
    [InlineData(-7.5600, 49.9600, 1398.2999115829, 21477.17948984)]
    [InlineData(1.7800, 60.8400, 605500.218278, 1223378.4826373)]
    public void CoordinateProjection_ProducesResultsWithinErrorMargin(
        double longitude,
        double latitude,
        double expectedLongitude,
        double expectedLatitude)
    {
        var sut = new Proj4SpatialProjectionService();

        var result = sut.Wgs84ToOsgb36(longitude, latitude);

        WithinErrorMarginPercent(result.longitude, expectedLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.latitude, expectedLatitude, ErrorMarginPercent);
    }

    [Theory]
    [InlineData(-7.5600, 49.9600, 1.7800, 60.8400, 1398.2999115829, 21477.17948984, 605500.218278, 1223378.4826373)]
    public void BoundingBoxProjection_ProducesResultsWithinErrorMargin(
        double westLongitude, double southLatitude, double eastLongitude, double northLatitude,
        double expectedWestLongitude, double expectedSouthLatitude, double expectedEastLongitude, double expectedNorthLatitude)
    {
        var sut = new Proj4SpatialProjectionService();

        var result = sut.Wgs84ToOsgb36(westLongitude, southLatitude, eastLongitude, northLatitude);

        WithinErrorMarginPercent(result.westLongitude, expectedWestLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.southLatitude, expectedSouthLatitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.eastLongitude, expectedEastLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.northLatitude, expectedNorthLatitude, ErrorMarginPercent);
    }

    [Theory]
    [InlineData(1398.2999115829, 21477.17948984, 605500.218278, 1223378.4826373, -7.5600, 49.9600, 1.7800, 60.8400)]
    public void BoundingBoxProjection_Inverse_ProducesResultsWithinErrorMargin(
        double westLongitude, double southLatitude, double eastLongitude, double northLatitude,
        double expectedWestLongitude, double expectedSouthLatitude, double expectedEastLongitude, double expectedNorthLatitude)
    {
        var sut = new Proj4SpatialProjectionService();

        var result = sut.Osgb36ToWgs84(westLongitude, southLatitude, eastLongitude, northLatitude);

        WithinErrorMarginPercent(result.westLongitude, expectedWestLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.southLatitude, expectedSouthLatitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.eastLongitude, expectedEastLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.northLatitude, expectedNorthLatitude, ErrorMarginPercent);
    }

    private void WithinErrorMarginPercent(double actual, double expected, double percent)
    {
        Assert.InRange(actual, expected * (1 - Math.CopySign(percent / 100, expected)), expected * (1 + Math.CopySign(percent / 100, expected)));
    }
}