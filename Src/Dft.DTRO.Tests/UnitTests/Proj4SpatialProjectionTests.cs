namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class Proj4SpatialProjectionTests
{
    private const double ErrorMarginPercent = 1.2;

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
        Proj4SpatialProjectionService sut = new();

        Coordinates result = sut.Wgs84ToOsgb36(longitude, latitude);

        WithinErrorMarginPercent(result.Longitude, expectedLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.Latitude, expectedLatitude, ErrorMarginPercent);
    }

    [Theory]
    [InlineData(-7.5600, 49.9600, 1.7800, 60.8400, 1398.2999115829, 21477.17948984, 605500.218278, 1223378.4826373)]
    public void BoundingBoxProjection_ProducesResultsWithinErrorMargin(
        double westLongitude, double southLatitude, double eastLongitude, double northLatitude,
        double expectedWestLongitude, double expectedSouthLatitude, double expectedEastLongitude, double expectedNorthLatitude)
    {
        Proj4SpatialProjectionService sut = new();

        BoundingBox result = sut.Wgs84ToOsgb36(westLongitude, southLatitude, eastLongitude, northLatitude);

        WithinErrorMarginPercent(result.WestLongitude, expectedWestLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.SouthLatitude, expectedSouthLatitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.EastLongitude, expectedEastLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.NorthLatitude, expectedNorthLatitude, ErrorMarginPercent);
    }

    [Theory]
    [InlineData(1398.2999115829, 21477.17948984, 605500.218278, 1223378.4826373, -7.5600, 49.9600, 1.7800, 60.8400)]
    public void BoundingBoxProjection_Inverse_ProducesResultsWithinErrorMargin(
        double westLongitude, double southLatitude, double eastLongitude, double northLatitude,
        double expectedWestLongitude, double expectedSouthLatitude, double expectedEastLongitude, double expectedNorthLatitude)
    {
        Proj4SpatialProjectionService sut = new();

        BoundingBox result = sut.Osgb36ToWgs84(westLongitude, southLatitude, eastLongitude, northLatitude);

        WithinErrorMarginPercent(result.WestLongitude, expectedWestLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.SouthLatitude, expectedSouthLatitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.EastLongitude, expectedEastLongitude, ErrorMarginPercent);
        WithinErrorMarginPercent(result.NorthLatitude, expectedNorthLatitude, ErrorMarginPercent);
    }

    private void WithinErrorMarginPercent(double actual, double expected, double percent)
    {
        Assert.InRange(actual, expected * (1 - Math.CopySign(percent / 100, expected)), expected * (1 + Math.CopySign(percent / 100, expected)));
    }
}