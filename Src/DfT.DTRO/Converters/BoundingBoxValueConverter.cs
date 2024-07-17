using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

namespace DfT.DTRO.Converters;

public class BoundingBoxValueConverter : ValueConverter<BoundingBox, NpgsqlBox>
{
    public BoundingBoxValueConverter()
        : base(
        boundingBox => new NpgsqlBox(boundingBox.NorthLatitude, boundingBox.EastLongitude, boundingBox.SouthLatitude, boundingBox.WestLongitude),
        npgBox => new BoundingBox(npgBox.Left, npgBox.Bottom, npgBox.Right, npgBox.Top))
    {
    }
}
