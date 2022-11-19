using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos;

public class PlatformReadDto
{
    public int Id { get; set; }

    public string Name { get; set; }
}