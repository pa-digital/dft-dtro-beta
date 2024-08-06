namespace DfT.DTRO.Converters;

/// <summary>
/// Defines conversion between <see cref="BoundingBox"/> structure and
/// its <seealso cref="NpgsqlBox"/> representation in the database.
/// </summary>
public class BoundingBoxValueConverter : ValueConverter<BoundingBox, NpgsqlBox>
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public BoundingBoxValueConverter()
        : base(
        boundingBox => new NpgsqlBox(boundingBox.NorthLatitude, boundingBox.EastLongitude, boundingBox.SouthLatitude, boundingBox.WestLongitude),
        npgBox => new BoundingBox(npgBox.Left, npgBox.Bottom, npgBox.Right, npgBox.Top))
    {
    }
}
