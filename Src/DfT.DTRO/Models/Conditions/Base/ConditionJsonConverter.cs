using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions.Base;

public class ConditionJsonConverter : JsonConverter<Condition>
{
    public override Condition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        if (json is null)
        {
            return null;
        }

        if (json is not JsonObject jsonObject)
        {
            throw new InvalidOperationException("The Condition must be an object");
        }

        if (jsonObject.ContainsKey("Condition"))
        {
            return json.Deserialize<ConditionSet>(options);
        }

        if (jsonObject.ContainsKey("roadType") ||
            jsonObject.ContainsKey("RoadCondition"))
        {
            return json.Deserialize<RoadCondition>(options);
        }

        if (jsonObject.ContainsKey("NumbersOfOccupants") || jsonObject.ContainsKey("disabledWithPermit"))
        {
            return json.Deserialize<OccupantCondition>(options);
        }

        if (jsonObject.ContainsKey("driverCharacteristicsType")
            || jsonObject.ContainsKey("licenseCharacteristics")
            || jsonObject.ContainsKey("ageOfDriver")
            || jsonObject.ContainsKey("timeDriversLicenseHeld"))
        {
            return json.Deserialize<DriverCondition>(options);
        }

        if (jsonObject.ContainsKey("accessConditionType") || jsonObject.ContainsKey("otherAccessRestriction"))
        {
            return json.Deserialize<AccessCondition>(options);
        }

        if (jsonObject.ContainsKey("type"))
        {
            return json.Deserialize<PermitCondition>(options);
        }

        if (jsonObject.ContainsKey("VehicleCharacteristics"))
        {
            return json.Deserialize<VehicleCharacteristic>(options);
        }

        if (jsonObject.ContainsKey("TimeValidity"))
        {
            return json.Deserialize<TimeValidity>(options);
        }


        throw new JsonException("Unknown condition type.");
    }

    public override void Write(Utf8JsonWriter writer, Condition value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}