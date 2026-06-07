using Object3DLibrary.Entites;
using System.Globalization;
using System.Numerics;

namespace Object3DLibrary;

public static class ObjectParser
{
    public static Object3D Parse(string objContent)
    {
        var obj = new Object3D();
        var lines = objContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("v "))
            {
                var vertex = ParseVertex(trimmedLine.Substring(2));
                obj.Vertices.Add(vertex);
                continue;
            }

            if (trimmedLine.StartsWith("f "))
            {
                var face = ParseFaceLine(trimmedLine);
                obj.Faces.Add(face.ToArray());
                continue;
            }
        }
        return obj;
    }


    public static Vector3 ParseVertex(string vertexLine)
    {
        var parts = vertexLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            throw new FormatException($"Ligne de vertex invalide : '{vertexLine}'");

        var style = System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands;
        if (!float.TryParse(parts[0], style, CultureInfo.InvariantCulture, out var x) ||
            !float.TryParse(parts[1], style, CultureInfo.InvariantCulture, out var y) ||
            !float.TryParse(parts[2], style, CultureInfo.InvariantCulture, out var z))
            throw new FormatException($"Coordonnées de vertex invalides : '{vertexLine}'");

        return new Vector3(x, y, z);
    }

    // Parse "f" line; vertexCount/texCount/normCount sont optionnels et nécessaires
    // seulement si la ligne utilise des indices négatifs.
    public static List<FaceVertex> ParseFaceLine(string line, int vertexCount = -1, int texCount = -1, int normCount = -1)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentException(nameof(line));

        line = line.Trim();
        if (!line.StartsWith("f ") && !line.StartsWith("f\t"))
            throw new ArgumentException("La ligne doit commencer par 'f'.");

        var parts = line.Substring(1).Trim().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        var result = new List<FaceVertex>(parts.Length);

        foreach (var p in parts)
        {
            // token attendus : v or v/vt or v/vt/vn

            var comps = p.Split('/');
            if (comps.Length < 1 || comps.Length > 3)
                throw new FormatException($"Token de face invalide : '{p}'");

            int? ParseIndex(string s, int baseCount)
            {
                if (string.IsNullOrEmpty(s))
                    return null;
                if (!int.TryParse(s, out var idx))
                    throw new FormatException($"Index invalide : '{s}'");

                if (idx < 0)
                {
                    if (baseCount < 0)
                        throw new InvalidOperationException("Impossible de résoudre un index négatif sans fournir le compte correspondant.");
                    // idx négatif : -1 -> last element -> baseCount - 1
                    return baseCount + idx;
                }
                // idx positif : convertir 1-based -> 0-based
                return idx - 1;
            }

            int? v = ParseIndex(comps[0], vertexCount);
            int? vt = comps.Length >= 2 ? ParseIndex(comps[1], texCount) : null;
            int? vn = comps.Length == 3 ? ParseIndex(comps[2], normCount) : null;

            if (!v.HasValue)
                throw new FormatException("Index de vertex manquant.");

            result.Add(new FaceVertex(v.Value, vt, vn));
        }

        return result;
    }
}
