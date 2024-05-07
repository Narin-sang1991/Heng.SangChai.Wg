using System;

namespace Cet.Hw.Core
{
    public class UserCriteria01
    {
        public Guid? Id { get; set; }
        public string Login { get; set; }
        public string LoginLike { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmailLike { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ParentId { get; set; }

        public string AutoCompleteText { get; set; }

    }
}
