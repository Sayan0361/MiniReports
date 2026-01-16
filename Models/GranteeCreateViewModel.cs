using System.Collections.Generic;

namespace MiniReportsProject.Models
{
    public class GranteeCreateViewModel
    {
        public string GranteeName { get; set; }
        public string SelectedTypeName { get; set; }
        public string Address { get; set; }

        // Strongly-typed list of available types
        public IEnumerable<EntityTypeModel> EntityTypes { get; set; }
    }
}