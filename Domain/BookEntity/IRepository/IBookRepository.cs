using Domain.BaseModel.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookEntity.IBookRepository;

public  interface IBookRepository : IBaseRepository<Book>
{
}
