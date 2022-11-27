using System.Collections.Generic;

namespace TFlic.Model.DTO;

public record Account (ulong Id, string Login, string Name, List<ulong> UserGroups);