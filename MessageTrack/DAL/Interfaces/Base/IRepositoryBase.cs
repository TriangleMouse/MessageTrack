using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTrack.DAL.Interfaces.Base
{
    /// <summary>
    /// Базовый интерфейс для репозиториев, содержащих общие методы доступа к данным.
    /// </summary>
    public interface IRepositoryBase<T> where T : class
    {
    }
}
