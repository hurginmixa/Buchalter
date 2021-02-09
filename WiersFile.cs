using System.Collections.Generic;

namespace Buchalter
{
    class WiersFile
    {
        private readonly List<IWier> _wiers;
        private readonly string _fileName;

        public WiersFile(string fileName)
        {
            _fileName = fileName;

            _wiers = Tools.LoadWiers(_fileName);
        }

        public IReadOnlyList<IWier> Wiers
        {
            get { return _wiers; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public void Insert(Wier wier)
        {
            _wiers.Insert(0, wier);
        }
    }
}