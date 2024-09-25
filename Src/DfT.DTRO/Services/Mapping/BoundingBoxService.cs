using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Mapping;

public class BoundingBoxService : IBoundingBoxService
{
    public BoundingBox SetBoundingBox(List<SemanticValidationError> errors, JObject obj, BoundingBox boundingBox)
    {
        JProperty children = obj.Children<JProperty>().ElementAt(1);
        IEnumerable<JToken> values;
        switch (children.Name)
        {
            case "PointGeometry":
                values = children.DescendantsAndSelf().Skip(3).FirstOrDefault();
                boundingBox = ValidateAgainstBoundingBox(values);
                break;
            case "LinearGeometry":
                values = children.DescendantsAndSelf().Skip(7).FirstOrDefault();
                boundingBox = ValidateAgainstBoundingBox(values);
                break;
            case "Polygon":
                values = children.DescendantsAndSelf().LastOrDefault();
                boundingBox = ValidateAgainstBoundingBox(values);
                break;
            case "DirectedLinear":
                values = children.DescendantsAndSelf().LastOrDefault();
                boundingBox = ValidateAgainstBoundingBox(values);
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = $"Geometry is not one of accepted type: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
        }

        return boundingBox;
    }

    private static BoundingBox ValidateAgainstBoundingBox(IEnumerable<JToken> values)
    {
        List<Coordinates> coordinates = new();

        List<string> points = values
            .Value<string>()
            .Split(" ")
            .Select(it => it
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", ""))
            .ToList();

        for (int index = 0; index < points.Count; index++)
        {
            Coordinates coordinate = new();
            coordinate.Longitude = points[index].AsInt();
            index++;
            coordinate.Latitude = points[index].AsInt();

            coordinates.Add(coordinate);
        }

        return BoundingBox.Wrapping(coordinates);
    }
}