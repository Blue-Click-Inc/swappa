namespace Swappa.Shared.DTOs
{
    public record CountryPaged(LocationMetaData MetaData, List<CountryData> Data);

    public record CountryData(Guid Id, string Name, string PhoneCode, string Currency, 
        string CurrencySymbol, string ISO2, string ISO3, string Capital, string Region,
        string Nationality, string Longitude, string Latitude, string Emoji, string EmojiUnicode,
        List<TimeZone> TimeZones);

    public record StateData(Guid Id, string Name, Guid CountryId, string CountryCode, string ISO2, 
        string Type, string Longitude, string Latitude);
    
    public record CityData(Guid Id, string Name, Guid StateId, Guid CountryId, string Longitude, 
        string Latitude);
    
    public record LocationMetaData(int Page, int Size, int Pages, bool HasNext, bool HasPrevious, long Count);
    
    public record TimeZone(string ZoneName, long GMTOffset, string GMTOffsetName, string Abbreviation, string TZName);          
}
