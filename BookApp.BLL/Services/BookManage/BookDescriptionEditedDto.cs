using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.BookManage
{
    public class BookDescriptionEditedDto
    {
        public Guid BookId { get; set; }
        [Required]
        [StringLength(
            maximumLength: DomainConstants.BookDescriptionMaxLenght,
            MinimumLength = DomainConstants.BookDescriptionMinLenght)]
        public string Description { get; set; }
    }
}
