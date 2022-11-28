namespace TFlic.Model.DTO;

public record Response<TValue>(string Message, TValue Value);