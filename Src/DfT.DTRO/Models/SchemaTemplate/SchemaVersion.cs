using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DfT.DTRO.Models.SchemaTemplate;

/// <summary>
/// Schema version in semantic versioning format.
/// </summary>
public class SchemaVersion : IComparable<SchemaVersion>
{
    private const string ValidationPattern = @"^\d+\.\d+\.\d+$";

    private int Major { get; }

    private int Minor { get; }

    private int Patch { get; }

    /// <summary>
    /// Creates a SchemaVersion object from string.
    /// </summary>
    /// <param name="version">Schema version string.</param>
    /// <exception cref="InvalidOperationException">Thrown when schema version is invalid.</exception>
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

    /// <summary>
    /// Creates a SchemaVersion object from three version components.
    /// </summary>
    /// <param name="major">Semver major.</param>
    /// <param name="minor">Semver minor.</param>
    /// <param name="patch">Semver patch.</param>
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

    /// <inheritdoc />
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

    /// <summary>
    /// Converts string to schema version instance.
    /// </summary>
    /// <param name="version">Schema version <see cref="string" />.</param>
    /// <returns>A <see cref="SchemaVersion" /> instance.</returns>
    public static implicit operator SchemaVersion(string version)
    {
        return new SchemaVersion(version);
    }

    /// <summary>
    /// Determines whether one instance of <see cref="SchemaVersion" /> is less than another.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the first instance is less than the second instance; otherwise, false.</returns>
    public static bool operator <(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) < 0;
    }

    /// <summary>
    /// Determines whether one instance of <see cref="SchemaVersion" /> is greater than another.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the first instance is greater than the second instance; otherwise, false.</returns>
    public static bool operator >(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) > 0;
    }

    /// <summary>
    /// Determines whether one instance of <see cref="SchemaVersion" /> is less than or equal to another.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the first instance is less than or equal to the second instance; otherwise, false.</returns>
    public static bool operator <=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) <= 0;
    }

    /// <summary>
    /// Determines whether one instance of <see cref="SchemaVersion" /> is greater than or equal to another.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the first instance is greater than or equal to the second instance; otherwise, false.</returns>
    public static bool operator >=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) >= 0;
    }

    /// <summary>
    /// Determines whether two instances of <see cref="SchemaVersion" /> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the instances are equal; otherwise, false.</returns>
    public static bool operator ==(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) == 0;
    }

    /// <summary>
    /// Determines whether two instances of <see cref="SchemaVersion" /> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="SchemaVersion" /> instance to compare.</param>
    /// <param name="right">The second <see cref="SchemaVersion" /> instance to compare.</param>
    /// <returns>True if the instances are not equal; otherwise, false.</returns>
    public static bool operator !=(SchemaVersion left, SchemaVersion right)
    {
        return Comparer<SchemaVersion>.Default.Compare(left, right) != 0;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    private bool Equals(SchemaVersion other)
    {
        return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
    }
}