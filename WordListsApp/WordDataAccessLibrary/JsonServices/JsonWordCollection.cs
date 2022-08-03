﻿using Newtonsoft.Json;

namespace WordDataAccessLibrary.JsonServices;
public class JsonWordCollection : IJsonWordCollection
{
    public string Name
    {
        get => _name;
        set => _name = (value is null) ? string.Empty : value;
    }

    public string Description
    {
        get => _description;
        set => _description = (value is null) ? string.Empty : value;
    }

    public string LanguageHeaders
    {
        get => _languageHeaders;
        set => _languageHeaders = (value is null) ? string.Empty : value;
    }

    public ExportWordPair[] WordPairs 
    { 
        get => _wordPairs;
        set => _wordPairs = value ?? Array.Empty<ExportWordPair>(); 
    }




    public void FromWordCollection(WordCollection collection)
    {
        if (collection is null) return;
        FromWordCollectionOwner(collection.Owner);
        WordPairsFromList(collection.WordPairs);
    }

    public void FromWordCollectionOwner(WordCollectionOwner owner)
    {
        if (owner is null) return;
        Name = owner.Name;
        Description = owner.Description;
        LanguageHeaders = owner.LanguageHeaders;
    }

    public void WordPairsFromList(List<WordPair> pairs)
    {
        if (pairs is null)
        {
            WordPairs = Array.Empty<ExportWordPair>();
            return;
        }

        List<ExportWordPair> exportPairs = new();
        foreach (WordPair pair in pairs)
        {
            if (pair is null) continue;
            exportPairs.Add(ExportWordPair.FromWordPair(pair));
        }
        WordPairs = exportPairs.ToArray();
    }

    /// <summary>
    /// Get object as json string, Object must have name or InvalidDataException is thrown
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public string GetAsJson()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new InvalidDataException($"{nameof(Name)} should not be null or empty");
        }
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
  
    /// <param name="objectAsJson"></param>
    /// <returns>JsonWordCollection matching string or null if argument null or otherwise bad</returns>
    public static JsonWordCollection ParseFromJson(string objectAsJson)
    {
        if (objectAsJson is null) return null;
        try
        {
            return JsonConvert.DeserializeObject<JsonWordCollection>(objectAsJson);
        }
        catch (JsonReaderException)
        {
            return null;
        }
    }



    string _name = string.Empty;
    string _description = string.Empty;
    string _languageHeaders = string.Empty;
    ExportWordPair[] _wordPairs = Array.Empty<ExportWordPair>();
}
