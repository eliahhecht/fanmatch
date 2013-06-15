using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanMatchTests
{
    class IdGenerator
    {
        private int lastId = 0;

        public int GetId()
        {
            return lastId++;
        }
    }
}
