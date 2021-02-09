using System.Collections.Generic;
using Buchalter.Types;

namespace Buchalter
{
    internal class SctMoving
    {
        public Sum SNach;

        public Sum SDebt
        {
            get
            {
                Sum ret = new Sum();
                foreach (Wire wire in myDebList)
                {
                    ret += wire.Sum;
                }

                return ret;
            }
        }

        public Sum SKred
        {
            get
            {
                Sum ret = new Sum();
                foreach (Wire wire in myKrdList)
                {
                    ret += wire.Sum;
                }

                return ret;
            }
        }

        public Sum SKont => SNach + SDebt - SKred;

        public List<Wire> DebList => myDebList;

        public List<Wire> KrdList => myKrdList;

        private readonly List<Wire> myDebList = new List<Wire>();
        private readonly List<Wire> myKrdList = new List<Wire>();
    }
}
