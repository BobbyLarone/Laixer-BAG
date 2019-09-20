using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaixerGMLTest
{
    public interface ILoader
    {
        Task LoadAsync(List<BAG_Objects.BAGObject> bAGObjects);
        //Task Load(List<BAG_Objects.BAGObject> bAGObjects);
    }
}
