using System.Text.Json.Serialization;

namespace url_shorten_backend;

public class IdUrlPair {
    public string Id { get; set; }
    public string Url { get; set; }
    public string UploaderId { get; set; }
}

public class IdUrlPairDto(IdUrlPair pair) {
    public string Id { get; set; } = pair.Id;
    public string Url { get; set; } = pair.Url;
}