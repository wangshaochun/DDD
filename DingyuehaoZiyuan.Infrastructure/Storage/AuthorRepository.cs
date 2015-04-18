using DingyuehaoZiyuan.Domain;
using System.ComponentModel.Composition;

namespace DingyuehaoZiyuan.Infrastructure.Storage
{

    [Export(typeof(IAuthorRepository))]
    internal class AuthorRepository : Repository<Author>,IAuthorRepository { }
}
