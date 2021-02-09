using System.Collections.Generic;
using Buchalter.Types;

namespace Buchalter
{
    class SctMoving
    {
        public Sum SNach;

        public Sum SDebt
        {
            get
            {
                Sum ret = new Sum();
                foreach (Wier wier in myDebList)
                {
                    ret += wier.Sum;
                }

                return ret;
            }
        }

        public Sum SKred
        {
            get
            {
                Sum ret = new Sum();
                foreach (Wier wier in myKrdList)
                {
                    ret += wier.Sum;
                }

                return ret;
            }
        }

        public Sum SKont
        {
            get { return SNach + SDebt - SKred; }
        }

        public List<Wier> DebList
        {
            get { return myDebList; }
        }

        public List<Wier> KrdList
        {
            get { return myKrdList; }
        }

        private readonly List<Wier> myDebList = new List<Wier>();
        private readonly List<Wier> myKrdList = new List<Wier>();
    }
}
