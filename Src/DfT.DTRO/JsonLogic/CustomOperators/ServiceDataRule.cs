﻿using Json.Logic;
using Json.More;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DfT.DTRO.JsonLogic.CustomOperators;

/// <summary>
/// Retrieves data by key from a set of static data stored within the service.
/// </summary>
[Operator("service_data")]
[JsonConverter(typeof(ServiceDataRuleJsonConverter))]
public class ServiceDataRule : Rule
{
    /// <summary>
    /// A <see cref="Rule"/> that should evaluate to a string key of the data.
    /// </summary>
    protected internal Rule Selector { get; }

    public static ReadOnlyDictionary<string, JsonNode> Data => _data;

    private static readonly ReadOnlyDictionary<string, JsonNode> _data =
        new (new Dictionary<string, JsonNode>
        {
            {
                "swa_codes", JsonNode.Parse(
                    "[10, 11, 12, 13, 14, 16, 17, 18, 20, 30, 50, 60, 70, 114, 116, 119, 121, 230, 235, 240, 335, 340, 345, 350, 355, 360, 430, 435, 440, 535, 540, 650, 655, 660, 665, 724, 728, 734, 738, 835, 840, 900, 935, 940, 1050, 1055, 1155, 1160, 1165, 1260, 1265, 1350, 1355, 1440, 1445, 1585, 1590, 1595, 1600, 1770, 1775, 1780, 1850, 1855, 1900, 2001, 2002, 2003, 2004, 2114, 2275, 2280, 2371, 2372, 2373, 2460, 2465, 2470, 2500, 2600, 2741, 2745, 2840, 2845, 2935, 3055, 3060, 3061, 3100, 3240, 3245, 3300, 3450, 3455, 3500, 3600, 3700, 3800, 3935, 3940, 4205, 4210, 4215, 4220, 4225, 4230, 4235, 4240, 4245, 4250, 4305, 4310, 4315, 4320, 4325, 4405, 4410, 4415, 4420, 4505, 4510, 4515, 4520, 4525, 4605, 4610, 4615, 4620, 4625, 4630, 4635, 4705, 4710, 4715, 4720, 4725, 4900, 5030, 5060, 5090, 5120, 5150, 5180, 5210, 5240, 5270, 5300, 5330, 5360, 5390, 5420, 5450, 5480, 5510, 5540, 5570, 5600, 5630, 5660, 5690, 5720, 5750, 5780, 5810, 5840, 5870, 5900, 5930, 5960, 5990, 6805, 6810, 6815, 6820, 6825, 6830, 6835, 6840, 6845, 6850, 6855, 6905, 6910, 6915, 6920, 6925, 6930, 6935, 6940, 6945, 6950, 6955, 6980, 6981, 6982, 6983, 7001, 7002, 7003, 7004, 7005, 7006, 7007, 7008, 7009, 7010, 7011, 7012, 7015, 7016, 7019, 7025, 7026, 7027, 7028, 7029, 7030, 7031, 7032, 7033, 7034, 7035, 7036, 7037, 7039, 7040, 7041, 7042, 7043, 7044, 7045, 7047, 7048, 7049, 7050, 7051, 7052, 7053, 7055, 7056, 7057, 7058, 7059, 7060, 7061, 7062, 7063, 7064, 7065, 7066, 7067, 7068, 7070, 7072, 7073, 7074, 7075, 7076, 7077, 7078, 7079, 7080, 7081, 7082, 7083, 7084, 7085, 7086, 7087, 7089, 7090, 7091, 7092, 7093, 7094, 7095, 7096, 7097, 7098, 7099, 7100, 7101, 7102, 7103, 7104, 7105, 7106, 7107, 7108, 7109, 7110, 7111, 7112, 7113, 7114, 7115, 7116, 7117, 7118, 7119, 7120, 7121, 7122, 7123, 7124, 7125, 7126, 7127, 7128, 7129, 7130, 7142, 7145, 7146, 7147, 7148, 7149, 7150, 7151, 7152, 7153, 7154, 7155, 7156, 7157, 7158, 7159, 7160, 7161, 7162, 7163, 7164, 7165, 7166, 7167, 7168, 7169, 7170, 7171, 7172, 7173, 7174, 7175, 7176, 7177, 7179, 7180, 7181, 7182, 7183, 7184, 7185, 7186, 7187, 7188, 7189, 7190, 7191, 7192, 7193, 7194, 7198, 7203, 7205, 7206, 7207, 7208, 7209, 7210, 7211, 7212, 7213, 7214, 7215, 7216, 7217, 7218, 7219, 7220, 7221, 7222, 7223, 7224, 7225, 7226, 7227, 7228, 7229, 7230, 7231, 7232, 7233, 7234, 7235, 7236, 7237, 7239, 7240, 7241, 7242, 7243, 7244, 7245, 7246, 7247, 7248, 7249, 7250, 7251, 7252, 7253, 7254, 7255, 7256, 7257, 7258, 7259, 7260, 7261, 7262, 7263, 7264, 7265, 7266, 7267, 7268, 7269, 7270, 7271, 7272, 7273, 7274, 7275, 7276, 7277, 7278, 7279, 7281, 7283, 7284, 7285, 7286, 7287, 7294, 7297, 7299, 7304, 7305, 7307, 7308, 7309, 7310, 7311, 7312, 7313, 7314, 7315, 7316, 7318, 7319, 7320, 7321, 7322, 7323, 7324, 7325, 7326, 7327, 7328, 7329, 7330, 7331, 7332, 7333, 7334, 7335, 7336, 7337, 7338, 7339, 7340, 7341, 7342, 7343, 7344, 7345, 7346, 7347, 7348, 7349, 7350, 7351, 7352, 7353, 7354, 7355, 7356, 7357, 7358, 7359, 7360, 7361, 7362, 7363, 7364, 7365, 7366, 7367, 7368, 7369, 7370, 7371, 7372, 7373, 7374, 7375, 7376, 7377, 7378, 7379, 7380, 7381, 7383, 7384, 7385, 7386, 7387, 7388, 7389, 7390, 7391, 7392, 7393, 7394, 7395, 7396, 7397, 7398, 7399, 7500, 7501, 7502, 7503, 7504, 7505, 7506, 7507, 7508, 7509, 7510, 7511, 7513, 7514, 7515, 7516, 7517, 7518, 7519, 7520, 7521, 7522, 7523, 7524, 7525, 7526, 7527, 7528, 7529, 7530, 7531, 7532, 7533, 7534, 7535, 7536, 7537, 7538, 7539, 7540, 7541, 7542, 7543, 7544, 7545, 7546, 7547, 7548, 7549, 7550, 7551, 7552, 7553, 7554, 7555, 7556, 7557, 7558, 7559, 7560, 7561, 7563, 7564, 7565, 7566, 7567, 7568, 9100, 9101, 9102, 9103, 9104, 9105, 9106, 9107, 9108, 9109, 9110, 9111, 9113, 9114, 9115, 9117, 9118, 9120, 9121, 9122, 9123, 9124, 9126, 9127, 9128, 9129, 9131, 9132, 9133, 9135, 9136, 9137, 9138, 9139, 9234, 9235, 9236, 9998, 9999]")
            }
        });

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="selector">A <see cref="Rule"/> that should evaluate to a string key of the data.</param>
    public ServiceDataRule(Rule selector)
    {
        Selector = selector;
    }

    /// <inheritdoc/>
    public override JsonNode Apply(JsonNode data, JsonNode contextData = null)
    {
        string key;

        var selector = Selector.Apply(data, contextData);

        try
        {
            key = selector.Deserialize<string>();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Key must evaluate to a string value.", e);
        }

        if (!_data.TryGetValue(key, out var value))
        {
            throw new InvalidOperationException($"No data found under key '{value}'");
        }

        return value.Copy();
    }
}

/// <summary>
/// Converts <see cref="ServiceDataRule"/> to and from JSON.
/// </summary>
public class ServiceDataRuleJsonConverter : JsonConverter<ServiceDataRule>
{
    /// <inheritdoc/>
    public override ServiceDataRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);

        var parameters = node is JsonArray
            ? node.Deserialize<Rule[]>()
            : new[] { node.Deserialize<Rule>() ! };

        if (parameters is not { Length: 1 })
        {
            throw new JsonException("The service_data rule needs an array with a single parameter.");
        }

        return new ServiceDataRule(parameters[0]);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ServiceDataRule value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("service_data");
        writer.WriteRule(value.Selector, options);
        writer.WriteEndObject();
    }
}
