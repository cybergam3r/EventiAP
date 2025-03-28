using System.Text.Json.Serialization;

public class Evento
{
    public int EventoId { get; set; }
    public string Titolo { get; set; }
    public DateTime Data { get; set; }
    public string Luogo { get; set; }
    public int ArtistaId { get; set; }
    [JsonIgnore]
    public Artista Artista { get; set; }
    public ICollection<Biglietto> Biglietti { get; set; }
}

