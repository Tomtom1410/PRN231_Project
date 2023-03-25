using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos
{
    public class CourseAccountDto
    {
        public long AccountId { get; set; }

        public long CourseId { get; set; }

        public bool? IsAuthor { get; set; }
    }
}
