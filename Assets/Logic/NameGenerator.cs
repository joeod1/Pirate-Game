using Assets;
using Assets.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public static NameMap LoadFromFile(string path)
    {
        string contents = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<NameMap>(contents);
    }

    // Dictionary<string, NameMap> nameMaps = new Dictionary<string, NameMap>();
    public static string GenerateName(float2 pos, NameMap map)
    {
        float randomValue = SeededRandom.RangeFloat(pos, 0f, 1f);
        int3 nameAssembly = new int3(); // (-fix, name, suffix 0 or prefix 1)
        print(randomValue);
        // Select the default name
        nameAssembly[2] = (int)(randomValue * 2); // whether prefix (1) or suffix (0)
        if (nameAssembly[2] == 1)
            nameAssembly[0] = (int)(randomValue * map.prefixes.Count); // prefix;
        else
            nameAssembly[0] = (int)(randomValue * map.suffixes.Count); // suffix
        nameAssembly[1] = (int)(randomValue * map.names.Count); // name

        // Iterate main part until name is unique OR we loop back to where we started
        int og = nameAssembly[1];
        while (map.existingNames.Contains(nameAssembly) && (nameAssembly[1] + 1 != og)) 
            nameAssembly[1] = 
                (nameAssembly[1] + 1) % map.names.Count;

        // Iterate -fix until the name is unique OR we loop back to where we started
        og = nameAssembly[0];
        int limit = (nameAssembly[2] == 1) ? map.prefixes.Count : map.suffixes.Count;
        while (map.existingNames.Contains(nameAssembly) && (nameAssembly[0] + 1 != og)) nameAssembly[0] = (nameAssembly[0] + 1) % limit;
        
        // Sad
        if (map.existingNames.Contains(nameAssembly))
        {
            // print("Failed to make a unique name for " + i);
        }
        map.existingNames.Add(nameAssembly);

        // Convert name to string
        string name;
        if (nameAssembly[2] == 1)
        {
            name = map.prefixes[nameAssembly[0]] + " " + map.names[nameAssembly[1]];
        }
        else
        {
            print(map.names.Count);
            print(nameAssembly[1]);
            print(map.suffixes.Count);
            print(nameAssembly[0]);
            name = map.names[nameAssembly[1]] + " " + map.suffixes[nameAssembly[0]];
        }
        return name;
    }
}
