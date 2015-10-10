using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelix3D
{
    

    public class ModelPartMgr
    {
        private ModelPartMgr()
        {
            
        }

        public static ModelPartMgr Instance
        {
            get { return Singleton<ModelPartMgr>.Instance; }
        }


    }
}
