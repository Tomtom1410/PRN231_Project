using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos
{
	public class DocumentDto
	{
        public long Id { get; set; }

        public string? DocumentName { get; set; }

        public string? DocumentOriginalName { get; set; }

        public string? ContentType { get; set; }

        public string? PathFile { get; set; }

        public long? CourseId { get; set; }

        public long? AccountId { get; set; }
        public AccountDto? Author { get; set; }
    }
}
