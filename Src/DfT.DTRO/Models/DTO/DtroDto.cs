public class DtroCountResponse
{
    public int Count { get; set; }
}

public class ValidateDTROByIdRequest
{
    public string Id { get; set; }
}

public class ValidatedDtroResponse
{
    public bool Valid { get; set; }
}