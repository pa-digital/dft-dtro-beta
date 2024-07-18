using System.Text.RegularExpressions;

namespace DfT.DTRO.Models.SchemaTemplate;

public class SchemaVersion : IComparable<SchemaVersion>
{
    private const string ValidationPattern = @"^\d+\.\d+\.\d+$";

    private int Major { get; }

    private int Minor { get; }

    private int Patch { get; }

    public SchemaVersion(string version)
    {
        if (version is null)
        {
            throw new ArgumentNullException(nameof(version));
        }

        if (!Regex.IsMatch(version, ValidationPattern))
        {
            throw new InvalidOperationException("Invalid schema version format.");
        }

        string[] splitVersion = version.Split(".");

        Major = int.Parse(splitVersion[0]);
        Minor = int.Parse(splitVersion[1]);
        Patch = int.Parse(splitVersion[2]);
    }

    public SchemaVersion(int major, int minor, int patch)
    {
        if (major < 0 || minor < 0 || patch < 0)
        {
            throw new InvalidOperationException("Versions must be greater than or equal to 0.");
        }

        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public int CompareTo(SchemaVersion other)
    {
        if (Major == other.Major && Minor == other.Minor && Patch == other.Patch)
        {
            return 0;
        }

        if (Major < other.Major)
        {
            return -1;
        }

        if (Major == other.Major && Minor < other.Minor)
        {
            return -1;
        }

        if (Major == other.Major && Minor == other.Minor &&
            Patch < other.Patch)
        {
            return -1;
        }

        return 1;
    }

    public static implicit operator SchemaVersion(string version)
    {
        return new SchemaVersion(version);
    }

    public static bool operator <(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) < 0;
    }

    public static bool operator >(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) > 0;
    }

    public static bool operator <=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) <= 0;
    }

    public static bool operator >=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) >= 0;
    }

    public static bool operator ==(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) == 0;
    }

    public static bool operator !=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) != 0;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == typeof(SchemaVersion) && Equals((SchemaVersion)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch);
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    private bool Equals(SchemaVersion other)
    {
        return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
    }
}